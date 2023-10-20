using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    public class UmbracoBlockValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly Dictionary<Type, string> _attributeTypes;

        public UmbracoBlockValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback, Type attributeType, string errorMessagePropertyAlias)
        {
            if (attributeType is null)
            {
                throw new ArgumentNullException(nameof(attributeType));
            }

            if (string.IsNullOrEmpty(errorMessagePropertyAlias))
            {
                throw new ArgumentException($"'{nameof(errorMessagePropertyAlias)}' cannot be null or empty.", nameof(errorMessagePropertyAlias));
            }

            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));
            _attributeTypes = new Dictionary<Type, string> { { attributeType, errorMessagePropertyAlias } };
        }

        public UmbracoBlockValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, IPublishedValueFallback publishedValueFallback, Dictionary<Type, string> attributeTypes)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _publishedValueFallback = publishedValueFallback ?? throw new ArgumentNullException(nameof(publishedValueFallback));
            _attributeTypes = attributeTypes ?? throw new ArgumentNullException(nameof(attributeTypes));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.ValidationMetadata == null || context.ValidationMetadata.ValidatorMetadata == null || context.ValidationMetadata.ValidatorMetadata.Count == 0) { return; }
            _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
            if (umbracoContext == null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent == null) { return; }

            var blocks = umbracoContext.PublishedRequest.PublishedContent.FindOverridableBlockModels(_publishedValueFallback).FindBlocks(x => true, _publishedValueFallback);
            if (!blocks.Any()) { return; }

            UpdateValidationAttributeErrorMessages(blocks, context.ValidationMetadata.ValidatorMetadata, _attributeTypes);
        }

        internal static void UpdateValidationAttributeErrorMessages(IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, IList<object> validationAttributes, Dictionary<Type, string> attributeTypes)
        {
            foreach (var attribute in validationAttributes)
            {
                foreach (var attributeType in attributeTypes.Keys)
                {
                    if (attribute.GetType().IsAssignableTo(attributeType))
                    {
                        UpdateValidationAttributeErrorMessage(blocks, attribute, attributeTypes[attributeType]);
                    }
                }
            }
        }

        private static void UpdateValidationAttributeErrorMessage(IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, object attribute, string errorMessagePropertyAlias)
        {
            var validationAttribute = attribute as ValidationAttribute;
            if (validationAttribute == null) { return; }

            var boundBlocks = blocks.Where(x => x.Settings != null &&
                                              x.Settings.GetProperty(PropertyAliases.ModelProperty) != null &&
                                              x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == validationAttribute.ErrorMessage);
            if (boundBlocks == null) { return; }

            foreach (var block in boundBlocks)
            {
                string? customError;
                if (block.Content.ContentType.Alias == ElementTypeAliases.ErrorMessage)
                {
                    customError = block.Content.GetProperty(PropertyAliases.ErrorMessage)?.GetValue()?.ToString();
                }
                else
                {
                    customError = block.Settings.GetProperty(errorMessagePropertyAlias)?.GetValue()?.ToString();
                }
                if (!string.IsNullOrEmpty(customError))
                {
                    validationAttribute.ErrorMessage = customError;
                }
            }
        }
    }
}
