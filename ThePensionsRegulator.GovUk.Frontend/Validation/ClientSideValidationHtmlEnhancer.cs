using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ThePensionsRegulator.GovUk.Frontend.Validation
{
    public class ClientSideValidationHtmlEnhancer : IClientSideValidationHtmlEnhancer
    {
        private readonly IModelPropertyResolverCollection _modelPropertyResolvers;
        private readonly IStringLocalizerFactory? _factory;
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly IOptions<MvcDataAnnotationsLocalizationOptions> _options;
        private readonly IValidationAttributeAdapterProvider _validationAttributeAdapterProvider;

        public ClientSideValidationHtmlEnhancer(
            IModelPropertyResolverCollection modelPropertyResolvers,
            IModelMetadataProvider metadataProvider,
            IOptions<MvcDataAnnotationsLocalizationOptions> options,
            IValidationAttributeAdapterProvider validationAttributeAdapterProvider,
            IStringLocalizerFactory? factory = null)
        {
            _modelPropertyResolvers = modelPropertyResolvers ?? throw new ArgumentNullException(nameof(modelPropertyResolvers));

            _factory = factory;
            _metadataProvider = metadataProvider;
            _validationAttributeAdapterProvider = validationAttributeAdapterProvider;
            _options = options;
        }

        public string EnhanceHtml(string html,
            ViewContext viewContext,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessagePhone,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            // Get the input element that should always be there if a <govuk-input> child exists.
            var inputs = document.DocumentNode.SelectNodes("//input");
            if (inputs == null) inputs = document.DocumentNode.SelectNodes("//select");
            if (inputs == null) inputs = document.DocumentNode.SelectNodes("//textarea");

            if (inputs != null && inputs.Count > 0)
            {
                // Get the output of the <govuk-input-error-message> grandchild tag helper, if present.
                var errorMessage = document.DocumentNode.SelectSingleNode("//*[contains(concat(' ', @class, ' '),' govuk-error-message ')]");

                // If <govuk-input-error-message> is present, the <govuk-input> tag helper always adds error classes because it assumes
                // we put it there because we already detected an error on the server. But we might want it there at all times to enable 
                // client-side validation, so remove those classes if there is not actually an error.
                bool hasError = ErrorMessageTagHelperHasRenderedAnError(errorMessage);
                if (!hasError)
                {
                    RemoveErrorClasses(document, inputs);
                }

                // Add the data-val-* attributes for ASP.NET / jQuery validation to pick up
                AddClientSideValidationAttributes(viewContext, _modelPropertyResolvers, _metadataProvider, _factory,
                    _validationAttributeAdapterProvider,
                    _options, inputs, errorMessage?.Attributes,
                    errorMessageRequired,
                    errorMessageRegex,
                    errorMessageEmail,
                    errorMessagePhone,
                    errorMessageLength,
                    errorMessageMinLength,
                    errorMessageMaxLength,
                    errorMessageRange,
                    errorMessageCompare);
            }

            return document.DocumentNode.OuterHtml;
        }

        private static void RemoveErrorClasses(HtmlDocument html, HtmlNodeCollection inputs)
        {
            var errorContainer = html.DocumentNode.SelectSingleNode("//*[contains(@class,'govuk-form-group--error')]");
            if (errorContainer != null) { errorContainer.RemoveClass("govuk-form-group--error"); }

            foreach (var input in inputs)
            {
                input.RemoveClass("govuk-input--error");
                input.RemoveClass("govuk-textarea--error");
                input.RemoveClass("govuk-select--error");
            }
        }

        private static bool ErrorMessageTagHelperHasRenderedAnError(HtmlNode? errorMessage)
        {
            if (errorMessage == null) { return false; }
            foreach (var node in errorMessage.ChildNodes)
            {
                if (node.NodeType == HtmlNodeType.Element && !node.HasClass("govuk-visually-hidden") ||
                    node.NodeType == HtmlNodeType.Text && !string.IsNullOrWhiteSpace(node.InnerText))
                {
                    return true;
                }
            }

            return false;
        }

        private static void AddClientSideValidationAttributes(ViewContext viewContext,
            IModelPropertyResolverCollection modelPropertyResolvers,
            IModelMetadataProvider metadataProvider,
            IStringLocalizerFactory? stringLocalizerFactory,
            IValidationAttributeAdapterProvider validationAttributeAdapterProvider,
            IOptions<MvcDataAnnotationsLocalizationOptions>? options,
            HtmlNodeCollection targetElements,
            HtmlAttributeCollection? errorMessageAttributes,
            string? errorMessageRequired,
            string? errorMessageRegex,
            string? errorMessageEmail,
            string? errorMessagePhone,
            string? errorMessageLength,
            string? errorMessageMinLength,
            string? errorMessageMaxLength,
            string? errorMessageRange,
            string? errorMessageCompare)
        {
            if (viewContext == null || !viewContext.ClientValidationEnabled) { return; }

            IStringLocalizer? localizer = null;

            foreach (var targetElement in targetElements)
            {
                if (errorMessageAttributes != null)
                {
                    var targetElementId = targetElement.Attributes["id"]?.Value;
                    if (!string.IsNullOrEmpty(targetElementId))
                    {
                        errorMessageAttributes.Add("data-valmsg-for", targetElementId);
                        errorMessageAttributes.Add("data-valmsg-replace", "false");
                        errorMessageAttributes.Add("id", targetElementId + "-error");
                    }
                }

                var modelPropertyName = targetElement.Attributes["name"]?.Value;
                if (string.IsNullOrEmpty(modelPropertyName) || modelPropertyName == ".Day" || modelPropertyName == ".Month" || modelPropertyName == ".Year") { continue; }
                if (modelPropertyName.EndsWith(".Day") || modelPropertyName.EndsWith(".Month") || modelPropertyName.EndsWith(".Year"))
                {
                    // Dates are a special case because we have collect child properties but they should all resolve to the one parent property.
                    modelPropertyName = modelPropertyName.Substring(0, modelPropertyName.LastIndexOf("."));
                }

                var modelProperty = modelPropertyResolvers.ResolveModelProperty(viewContext, modelPropertyName);

                if (modelProperty.DeclaringType is not null)
                {
                    if (options?.Value?.DataAnnotationLocalizerProvider != null && stringLocalizerFactory != null)
                    {
                        // This will pass first non-null type (either containerType or modelType) to delegate.
                        // Pass the root model type(container type) if it is non null, else pass the model type.
                        localizer = options.Value.DataAnnotationLocalizerProvider(
                            modelProperty.DeclaringType,
                            stringLocalizerFactory);
                    }

                    var validateElement = false;

                    if (IsIntegerNumericType(modelProperty.PropertyType) || IsFloatingPointNumericType(modelProperty.PropertyType))
                    {
                        AddOrUpdateHtmlAttribute(targetElement, "type", "text");
                        AddOrUpdateHtmlAttribute(targetElement, "inputmode", "numeric");
                        if (IsIntegerNumericType(modelProperty.PropertyType))
                        {
                            AddOrUpdateHtmlAttribute(targetElement, "pattern", "[0-9]*");
                        }
                        else
                        {
                            AddOrUpdateHtmlAttribute(targetElement, "pattern", "[0-9.]*");
                        }
                    }

                    // Compare
                    var compareAttr = modelProperty.GetCustomAttributes<CompareAttribute>().FirstOrDefault();
                    if (compareAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageCompare, compareAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-equalto", errorMessage);
                            targetElement.Attributes.Add("data-val-equalto-other", compareAttr.OtherProperty);
                            validateElement = true;
                        }
                    }

                    // Email Address
                    var emailAttr = modelProperty.GetCustomAttributes<EmailAddressAttribute>().FirstOrDefault();
                    if (emailAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageEmail, emailAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-email", errorMessage);
                            AddOrUpdateHtmlAttribute(targetElement, "autocomplete", "email");
                            AddOrUpdateHtmlAttribute(targetElement, "type", "email");
                            validateElement = true;
                        }
                    }

                    // Phone
                    var phoneAttr = modelProperty.GetCustomAttributes<PhoneAttribute>().FirstOrDefault();
                    if (phoneAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessagePhone, phoneAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-phone", errorMessage);
                            AddOrUpdateHtmlAttribute(targetElement, "autocomplete", "tel");
                            AddOrUpdateHtmlAttribute(targetElement, "type", "tel");
                            validateElement = true;
                        }
                    }

                    // Max Length
                    var maxLengthAttr = modelProperty.GetCustomAttributes<MaxLengthAttribute>().FirstOrDefault();
                    if (maxLengthAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageMaxLength, maxLengthAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-maxlength", errorMessage);
                            targetElement.Attributes.Add("data-val-maxlength-max", maxLengthAttr.Length.ToString());
                            targetElement.Attributes.Add("maxlength", maxLengthAttr.Length.ToString());
                            validateElement = true;
                        }
                    }

                    //// Min Length
                    var minLengthAttr = modelProperty.GetCustomAttributes<MinLengthAttribute>().FirstOrDefault();
                    if (minLengthAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageMinLength, minLengthAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-minlength", errorMessage);
                            targetElement.Attributes.Add("data-val-minlength-min", minLengthAttr.Length.ToString());
                            validateElement = true;
                        }
                    }

                    // Range
                    var rangeAttr = modelProperty.GetCustomAttributes<RangeAttribute>().FirstOrDefault();
                    if (rangeAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageRange, rangeAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            var max = rangeAttr.Maximum.ToString();
                            var min = rangeAttr.Minimum.ToString();
                            targetElement.Attributes.Add("data-val-range", errorMessage);
                            if (max is not null) { targetElement.Attributes.Add("data-val-range-max", max); }
                            if (min is not null) { targetElement.Attributes.Add("data-val-range-min", min); }
                            validateElement = true;
                        }
                    }

                    // Regex
                    var regexAttr = modelProperty.GetCustomAttributes<RegularExpressionAttribute>().FirstOrDefault();
                    if (regexAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageRegex, regexAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-regex", errorMessage);
                            targetElement.Attributes.Add("data-val-regex-pattern", regexAttr.Pattern);
                            AddOrUpdateHtmlAttribute(targetElement, "pattern", regexAttr.Pattern);
                            validateElement = true;
                        }
                    }

                    // Required
                    var reqdAttr = modelProperty.GetCustomAttributes<RequiredAttribute>().FirstOrDefault();
                    if (reqdAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageRequired, reqdAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("required", "required");
                            targetElement.Attributes.Add("data-val-required", errorMessage);
                            validateElement = true;
                        }
                    }

                    // String Length
                    var strLenAttr = modelProperty.GetCustomAttributes<StringLengthAttribute>().FirstOrDefault();
                    if (strLenAttr != null)
                    {
                        var errorMessage = SelectBestErrorMessage(errorMessageLength, strLenAttr.ErrorMessage, localizer);
                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            targetElement.Attributes.Add("data-val-length", errorMessage);
                            targetElement.Attributes.Add("data-val-length-max", strLenAttr.MaximumLength.ToString());
                            targetElement.Attributes.Add("data-val-length-min", strLenAttr.MinimumLength.ToString());
                            targetElement.Attributes.Add("maxlength", strLenAttr.MaximumLength.ToString());
                            validateElement = true;
                        }
                    }

                    // Get anything else that inherits from ValidationAttribute
                    var baseValidationAttributes = modelProperty.GetCustomAttributes<ValidationAttribute>();
                    if (baseValidationAttributes != null && baseValidationAttributes.Count() > 0)
                    {
                        foreach (var baseValidationAttribute in baseValidationAttributes)
                        {
                            var adapter = validationAttributeAdapterProvider.GetAttributeAdapter(baseValidationAttribute, localizer);

                            if (adapter != null)
                            {
                                var attrDictionary = new Dictionary<string, string>();

                                // Get existing attributes - have to iterate to avoid duplication
                                foreach (var attribute in targetElement.Attributes)
                                {
                                    if (!attrDictionary.ContainsKey(attribute.Name))
                                    {
                                        attrDictionary.Add(attribute.Name, attribute.Value);
                                    }
                                }

                                // Get metadata
                                var metadata = metadataProvider.GetMetadataForType(modelProperty.DeclaringType);

                                // Now call the Adapter's AddValidation method. This merges existing attributes with anything already present
                                adapter!.AddValidation(
                                    new ClientModelValidationContext(viewContext, metadata, metadataProvider, attrDictionary)
                                    );

                                // Remove existing html attributes
                                targetElement.Attributes.RemoveAll();

                                // And add them back in again - this time with everything populated by the AddValidation method.
                                foreach (var attr in attrDictionary)
                                {
                                    targetElement.Attributes.Add(attr.Key, attr.Value);
                                }

                                validateElement = true;
                            }
                        }
                    }

                    if (validateElement)
                    {
                        if (!targetElement.Attributes.Contains("data-val"))
                        {
                            targetElement.Attributes.Add("data-val", "true");
                        }
                    }

                }
                else
                {
                    throw new InvalidOperationException($"To support client-side validation decorate your controller action with {nameof(ModelTypeAttribute)} to specify a different model type with a property named {modelPropertyName}.");
                }
            }
        }

        private static void AddOrUpdateHtmlAttribute(HtmlNode element, string name, string value)
        {
            if (element.Attributes.AttributesWithName(name).Any())
            {
                element.Attributes[name].Value = value;
            }
            else
            {
                element.Attributes.Add(name, value);
            }
        }

        private static string? SelectBestErrorMessage(string? fromTagHelperAttribute, string? fromDataAnnotationsAttribute, IStringLocalizer? localizer)
        {
            return string.IsNullOrEmpty(fromTagHelperAttribute) ?
                        (localizer != null && !string.IsNullOrEmpty(fromDataAnnotationsAttribute) ? localizer[fromDataAnnotationsAttribute!] : fromDataAnnotationsAttribute) :
                        fromTagHelperAttribute;
        }

        private static readonly HashSet<Type> IntegerNumericTypes = new HashSet<Type>
        {
            typeof(sbyte), typeof(byte),
            typeof(int), typeof(uint),
            typeof(short), typeof(ushort),
            typeof(long), typeof(ulong)

        };

        private static readonly HashSet<Type> FloatingPointNumericTypes = new HashSet<Type>
        {
            typeof(double), typeof(decimal), typeof(float)
        };

        private static bool IsFloatingPointNumericType(Type type)
        {
            return FloatingPointNumericTypes.Contains(type) ||
                   FloatingPointNumericTypes.Contains(Nullable.GetUnderlyingType(type)!);
        }

        private static bool IsIntegerNumericType(Type type)
        {
            return IntegerNumericTypes.Contains(type) ||
                   IntegerNumericTypes.Contains(Nullable.GetUnderlyingType(type)!);
        }
    }
}
