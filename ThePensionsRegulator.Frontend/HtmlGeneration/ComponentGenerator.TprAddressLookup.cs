using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.Frontend.TagHelpers;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprAddressLookupElement = "div";

        public virtual TagBuilder GenerateTprAddressLookup(AttributeDictionary? legendAttributes, IHtmlContent? legendContent, IHtmlContent? childContent, string? fieldsetDescribedBy, AddressLookupRole role, string? sameAsPrimaryCheckboxLabel)
        {
            var container = new TagBuilder(TprAddressLookupElement);
            container.AddCssClass("tpr-address-lookup");

            container.Attributes.Add("data-address-lookup-role", role.ToString().ToLower());

            if (role == AddressLookupRole.Secondary && !string.IsNullOrEmpty(sameAsPrimaryCheckboxLabel))
            {
                container.Attributes.Add("data-address-lookup-same-as-primary-checkbox-label", sameAsPrimaryCheckboxLabel);
            }

            var fieldSet = new TagBuilder("fieldset");
            fieldSet.AddCssClass("govuk-fieldset");
            if (!string.IsNullOrEmpty(fieldsetDescribedBy))
            {
                fieldSet.Attributes.Add("described-by", fieldsetDescribedBy);
            }

            if (legendContent is not null)
            {
                var legendTag = new TagBuilder("legend");
                if (legendAttributes is not null)
                {
                    legendTag.MergeAttributes(legendAttributes);
                }
                legendTag.MergeCssClass("govuk-fieldset__legend");

                legendTag.InnerHtml.AppendHtml(legendContent);
                legendTag.MergeCssClass("govuk-fieldset__legend--for-fieldset");

                fieldSet.InnerHtml.AppendHtml(legendTag);
            }

            if (childContent is not null)
            {
                fieldSet.InnerHtml.AppendHtml(childContent);
            }

            container.InnerHtml.AppendHtml(fieldSet);

            return container;
        }
    }
}
