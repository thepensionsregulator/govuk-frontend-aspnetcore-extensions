using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using GovUk.Frontend.Umbraco;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;

namespace ThePensionsRegulator.Frontend.Umbraco.Validation
{
    public class AddressLookupValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly Dictionary<Type, string> _attributeTypes;

        public AddressLookupValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback, Dictionary<Type, string> attributeTypes)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));
            _attributeTypes = attributeTypes ?? throw new ArgumentNullException(nameof(attributeTypes));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.ValidationMetadata is null || context.ValidationMetadata.ValidatorMetadata is null || context.ValidationMetadata.ValidatorMetadata.Count == 0) { return; }
            _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
            if (umbracoContext is null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent is null) { return; }

            var validationAttributes = context.ValidationMetadata.ValidatorMetadata.OfType<ValidationAttribute>().Where(x => !string.IsNullOrEmpty(x.ErrorMessage)).ToList();
            if (!validationAttributes.Any()) { return; }

            var addressLookupBlocks = umbracoContext.PublishedRequest.PublishedContent
                .FindOverridableBlockModels(_publishedValueFallback)
                .FindBlocksByContentTypeAlias(TprElementTypeAliases.AddressLookup, _publishedValueFallback);

            if (!addressLookupBlocks.Any()) { return; }

            UpdateValidationAttributeErrorMessages(addressLookupBlocks, validationAttributes, _attributeTypes);
        }

        internal static void UpdateValidationAttributeErrorMessages(IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> addressBlocks, IList<ValidationAttribute> validationAttributes, Dictionary<Type, string> attributeTypes)
        {
            foreach (var attribute in validationAttributes)
            {
                foreach (var attributeType in attributeTypes.Keys)
                {
                    if (attribute.GetType().IsAssignableTo(attributeType))
                    {
                        UpdateValidationAttributeErrorMessage(addressBlocks, attribute, attributeTypes[attributeType]);
                    }
                }
            }
        }

        private static void UpdateValidationAttributeErrorMessage(IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, ValidationAttribute attribute, string errorMessagePropertyAlias)
        {
            var errorMessageFound = false;
            foreach (var block in blocks)
            {
                if (errorMessageFound)
                {
                    break;
                }

                var propertyKeys = block.Settings.Properties.Where(x => x.Alias.ToLower().Contains(PropertyAliases.ModelProperty.ToLower()));

                foreach (var key in propertyKeys)
                {
                    var propertyValue = block.Settings.GetProperty(key.Alias)?.GetValue();

                    if (propertyValue is not null && propertyValue.Equals(attribute?.ErrorMessage))
                    {
                        var keySplit = key.Alias.Split($"{char.ToUpperInvariant(PropertyAliases.ModelProperty[0])}{PropertyAliases.ModelProperty[1..]}");
                        var firstBit = keySplit[0];
                        var desiredErrorMessagePropertyKey = $"{firstBit}{char.ToUpperInvariant(errorMessagePropertyAlias[0])}{errorMessagePropertyAlias[1..]}";

                        var customError = block.Settings.GetProperty(desiredErrorMessagePropertyKey)?.GetValue()?.ToString();

                        if (!string.IsNullOrEmpty(customError))
                        {
                            attribute.ErrorMessage = customError;
                            errorMessageFound = true;
                            break;
                        }
                    }
                }
            }
        }
    }
}
