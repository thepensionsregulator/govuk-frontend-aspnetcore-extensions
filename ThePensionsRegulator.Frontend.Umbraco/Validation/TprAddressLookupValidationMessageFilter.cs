using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Validation;

public sealed class TprAddressLookupValidationMessageFilter : IActionFilter
{
    private const int MaxObjectGraphDepth = 5;

    private readonly ITprAddressLookupValidationMessageResolver _resolver;
    private readonly IModelMetadataProvider _modelMetadataProvider;

    public TprAddressLookupValidationMessageFilter(
        ITprAddressLookupValidationMessageResolver resolver,
        IModelMetadataProvider modelMetadataProvider)
    {
        _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        _modelMetadataProvider = modelMetadataProvider ?? throw new ArgumentNullException(nameof(modelMetadataProvider));
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!HttpMethods.IsPost(context.HttpContext.Request.Method) ||
            !context.ModelState.Keys.Any(IsAddressFieldKey))
        {
            return;
        }

        var processedPrefixes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var addressModel in FindAddressModels(context.ActionArguments))
        {
            if (string.IsNullOrWhiteSpace(addressModel.Prefix) || !processedPrefixes.Add(addressModel.Prefix))
            {
                continue;
            }

            var content = _resolver.Resolve(addressModel.Prefix);
            if (content is null)
            {
                continue;
            }

            ReplaceDefaultAnnotationMessages(context.ModelState, addressModel, content);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    private void ReplaceDefaultAnnotationMessages(
        ModelStateDictionary modelState,
        AddressModel addressModel,
        TprAddressLookupContent content)
    {
        foreach (var property in addressModel.Address.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            var validationAttributes = property.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray();
            if (validationAttributes.Length == 0)
            {
                continue;
            }

            var key = JoinPrefix(addressModel.Prefix, property.Name);
            if (!modelState.TryGetValue(key, out var entry))
            {
                continue;
            }

            var displayName = _modelMetadataProvider
                .GetMetadataForProperty(addressModel.Address.GetType(), property.Name)
                .GetDisplayName();

            for (var errorIndex = 0; errorIndex < entry.Errors.Count; errorIndex++)
            {
                var error = entry.Errors[errorIndex];
                if (error.Exception is not null || string.IsNullOrWhiteSpace(error.ErrorMessage))
                {
                    continue;
                }

                foreach (var attribute in validationAttributes)
                {
                    var replacement = GetContentMessage(property.Name, attribute, content);
                    if (replacement is null || HasCustomAttributeMessage(attribute))
                    {
                        continue;
                    }

                    if (string.Equals(attribute.FormatErrorMessage(displayName), error.ErrorMessage, StringComparison.CurrentCulture))
                    {
                        entry.Errors[errorIndex] = new ModelError(replacement);
                        break;
                    }
                }
            }
        }
    }

    private static string? GetContentMessage(string propertyName, ValidationAttribute attribute, TprAddressLookupContent content)
    {
        if (attribute is RequiredAttribute)
        {
            return propertyName switch
            {
                nameof(ITprAddress.AddressLine1) => content.AddressLine1RequiredErrorMessage,
                nameof(ITprAddress.PostTown) => content.PostTownRequiredErrorMessage,
                nameof(ITprAddress.Postcode) => content.PostcodeRequiredErrorMessage,
                nameof(ITprAddress.CountryId) => content.CountryRequiredErrorMessage,
                _ => null
            };
        }

        if (attribute is MaxLengthAttribute)
        {
            return propertyName switch
            {
                nameof(ITprAddress.AddressLine1) => content.AddressLine1MaxLengthErrorMessage,
                nameof(ITprAddress.AddressLine2) => content.AddressLine2MaxLengthErrorMessage,
                nameof(ITprAddress.AddressLine3) => content.AddressLine3MaxLengthErrorMessage,
                nameof(ITprAddress.PostTown) => content.PostTownMaxLengthErrorMessage,
                nameof(ITprAddress.PostCounty) => content.CountyMaxLengthErrorMessage,
                nameof(ITprAddress.Postcode) => content.PostcodeMaxLengthErrorMessage,
                _ => null
            };
        }

        return null;
    }

    private static bool HasCustomAttributeMessage(ValidationAttribute attribute) =>
        attribute.ErrorMessage is not null ||
        attribute.ErrorMessageResourceName is not null ||
        attribute.ErrorMessageResourceType is not null;

    private static bool IsAddressFieldKey(string key)
    {
        var lastSeparator = key.LastIndexOf('.');
        var fieldName = lastSeparator >= 0 ? key[(lastSeparator + 1)..] : key;
        return AddressFieldNames.Contains(fieldName);
    }

    private static IEnumerable<AddressModel> FindAddressModels(IDictionary<string, object?> actionArguments)
    {
        foreach (var argument in actionArguments)
        {
            foreach (var addressModel in FindAddressModels(argument.Value, string.Empty, 0, new HashSet<object>(ReferenceEqualityComparer.Instance)))
            {
                yield return addressModel;
            }

            if (!string.IsNullOrWhiteSpace(argument.Key))
            {
                foreach (var addressModel in FindAddressModels(argument.Value, argument.Key, 0, new HashSet<object>(ReferenceEqualityComparer.Instance)))
                {
                    yield return addressModel;
                }
            }
        }
    }

    private static IEnumerable<AddressModel> FindAddressModels(object? value, string prefix, int depth, HashSet<object> visited)
    {
        if (value is ITprAddress address)
        {
            yield return new AddressModel(prefix, address);
            yield break;
        }

        if (value is null || depth >= MaxObjectGraphDepth || IsLeaf(value.GetType()) || !visited.Add(value))
        {
            yield break;
        }

        foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.CanRead || property.GetIndexParameters().Length != 0 ||
                typeof(IPublishedContent).IsAssignableFrom(property.PropertyType) ||
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                continue;
            }

            var propertyValue = property.GetValue(value);
            foreach (var addressModel in FindAddressModels(propertyValue, JoinPrefix(prefix, property.Name), depth + 1, visited))
            {
                yield return addressModel;
            }
        }

        visited.Remove(value);
    }

    private static bool IsLeaf(Type type) =>
        type.IsPrimitive || type.IsEnum || type.IsValueType || type == typeof(string);

    private static string JoinPrefix(string prefix, string propertyName) =>
        string.IsNullOrEmpty(prefix) ? propertyName : $"{prefix}.{propertyName}";

    private sealed record AddressModel(string Prefix, ITprAddress Address);

    private static readonly HashSet<string> AddressFieldNames = new(StringComparer.OrdinalIgnoreCase)
    {
        nameof(ITprAddress.AddressLine1),
        nameof(ITprAddress.AddressLine2),
        nameof(ITprAddress.AddressLine3),
        nameof(ITprAddress.PostTown),
        nameof(ITprAddress.PostCounty),
        nameof(ITprAddress.Postcode),
        nameof(ITprAddress.CountryId)
    };

}
