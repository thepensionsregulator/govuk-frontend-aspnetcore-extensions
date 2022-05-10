using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    public class UmbracoBlockListValidationMetadataProvider : IValidationMetadataProvider
    {
        private readonly IUmbracoContextAccessor _umbracoContextAccessor;
        private readonly string _documentTypeAlias;
        private readonly string _blockListAlias;

        public UmbracoBlockListValidationMetadataProvider(IUmbracoContextAccessor umbracoContextAccessor, string documentTypeAlias, string blockListAlias)
        {
            _umbracoContextAccessor = umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
            _documentTypeAlias = documentTypeAlias ?? throw new ArgumentNullException(nameof(documentTypeAlias));
            _blockListAlias = blockListAlias ?? throw new ArgumentNullException(nameof(blockListAlias));
        }

        public void CreateValidationMetadata(ValidationMetadataProviderContext context)
        {
            if (context.Attributes.Count == 0) { return; }
            _umbracoContextAccessor.TryGetUmbracoContext(out var umbracoContext);
            if (umbracoContext == null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent == null) { return; }
            if (umbracoContext.PublishedRequest?.PublishedContent.ContentType.Alias.ToUpperInvariant() != _documentTypeAlias.ToUpperInvariant()) { return; }

            var blockList = umbracoContext.PublishedRequest.PublishedContent.Value<BlockListModel>(_blockListAlias);
            if (blockList == null) { return; }

            foreach (var attribute in context.Attributes)
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
