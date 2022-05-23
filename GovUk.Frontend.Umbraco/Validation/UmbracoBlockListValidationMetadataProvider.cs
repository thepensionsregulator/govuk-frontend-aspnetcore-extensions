using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    public class UmbracoBlockListValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;

        public UmbracoBlockListValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.ValidationMetadata == null || context.ValidationMetadata.ValidatorMetadata == null || context.ValidationMetadata.ValidatorMetadata.Count == 0) { return; }
            _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
            if (umbracoContext == null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent == null) { return; }

            var blockLists = umbracoContext.PublishedRequest.PublishedContent.Properties.Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList).Select(x => x.Value<BlockListModel>(null));
            RecursivelyUpdateBlockLists(context.ValidationMetadata.ValidatorMetadata, blockLists);

        }

        private static void RecursivelyUpdateBlockLists(IList<object> validationAttributes, IEnumerable<BlockListModel> blockLists)
        {
            foreach (var blockList in blockLists)
            {
                foreach (var attribute in validationAttributes)
                {
                    if (attribute is RequiredAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageRequired"); }
                    if (attribute is RegularExpressionAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageRegex"); }
                    if (attribute is EmailAddressAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageEmail"); }
                    if (attribute is StringLengthAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageLength"); }
                    if (attribute is MinLengthAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageMinLength"); }
                    if (attribute is MaxLengthAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageMaxLength"); }
                    if (attribute is RangeAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageRange"); }
                    if (attribute is CompareAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageCompare"); }
                    if (attribute is CreditCardAttribute) { UpdateValidationAttributeErrorMessage(blockList, attribute, "errorMessageCreditCard"); }
                }

                RecursivelyUpdateBlockLists(validationAttributes, blockList.SelectMany(block => block.Content.Properties.Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList).Select(x => x.Value<BlockListModel>(null))));
            }
        }

        private static void UpdateValidationAttributeErrorMessage(BlockListModel blockList, object attribute, string errorMessagePropertyAlias)
        {
            var validationAttribute = attribute as ValidationAttribute;
            if (validationAttribute == null) { return; }

            var block = blockList.SingleOrDefault(x => x.Settings != null && x.Settings.Value<string>("modelProperty") == validationAttribute.ErrorMessage);
            if (block == null) { return; }

            var customError = block.Settings.Value<string>(errorMessagePropertyAlias);
            if (!string.IsNullOrEmpty(customError))
            {
                validationAttribute.ErrorMessage = customError;
            }
        }
    }
}
