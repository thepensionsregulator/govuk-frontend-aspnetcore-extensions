using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    public class UmbracoBlockListValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly Dictionary<Type, string> _attributeTypes;

        public UmbracoBlockListValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, Type attributeType, string errorMessagePropertyAlias)
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
            _attributeTypes = new Dictionary<Type, string> { { attributeType, errorMessagePropertyAlias } };
        }

        public UmbracoBlockListValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, Dictionary<Type, string> attributeTypes)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _attributeTypes = attributeTypes ?? throw new ArgumentNullException(nameof(attributeTypes));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.ValidationMetadata == null || context.ValidationMetadata.ValidatorMetadata == null || context.ValidationMetadata.ValidatorMetadata.Count == 0) { return; }
            _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
            if (umbracoContext == null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent == null) { return; }

            var blockLists = RecursivelyGetBlockLists(umbracoContext.PublishedRequest.PublishedContent.Properties);
            UpdateValidationAttributeErrorMessages(blockLists, context.ValidationMetadata.ValidatorMetadata, _attributeTypes);
        }

        internal IEnumerable<BlockListModel> RecursivelyGetBlockLists(IEnumerable<IPublishedProperty> properties)
        {
            var blockLists = new List<BlockListModel>();
            RecursivelyGetBlockLists(properties, blockLists);
            return blockLists;
        }

        private void RecursivelyGetBlockLists(IEnumerable<IPublishedProperty> properties, List<BlockListModel> allBlockLists)
        {
            var newBlockLists = properties.Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue()).Select(x => x.Value<BlockListModel>(null));
            if (newBlockLists.Any())
            {
                allBlockLists.AddRange(newBlockLists);

                foreach (var blockList in newBlockLists)
                {
                    RecursivelyGetBlockLists(blockList.SelectMany(block => block.Content.Properties), allBlockLists);
                }
            }
        }

        internal static void UpdateValidationAttributeErrorMessages(IEnumerable<BlockListModel> blockLists, IList<object> validationAttributes, Dictionary<Type, string> attributeTypes)
        {
            foreach (var blockList in blockLists)
            {
                foreach (var attribute in validationAttributes)
                {
                    foreach (var attributeType in attributeTypes.Keys)
                    {
                        if (attribute.GetType().IsAssignableTo(attributeType))
                        {
                            UpdateValidationAttributeErrorMessage(blockList, attribute, attributeTypes[attributeType]);
                        }
                    }
                }
            }
        }

        private static void UpdateValidationAttributeErrorMessage(BlockListModel blockList, object attribute, string errorMessagePropertyAlias)
        {
            var validationAttribute = attribute as ValidationAttribute;
            if (validationAttribute == null) { return; }

            var blocks = blockList.Where(x => x.Settings != null &&
                                              x.Settings.GetProperty(PropertyAliases.ModelProperty) != null &&
                                              x.Settings.GetProperty(PropertyAliases.ModelProperty).GetValue().ToString() == validationAttribute.ErrorMessage);
            if (blocks == null) { return; }

            foreach (var block in blocks)
            {
                var customError = block.Settings.GetProperty(errorMessagePropertyAlias).GetValue().ToString();
                if (!string.IsNullOrEmpty(customError))
                {
                    validationAttribute.ErrorMessage = customError;
                }
            }
        }
    }
}
