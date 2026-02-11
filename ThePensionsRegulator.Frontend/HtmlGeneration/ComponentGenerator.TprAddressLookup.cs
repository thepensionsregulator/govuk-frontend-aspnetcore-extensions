using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprAddressLookupElement = "div";
        internal const bool AddressLookupIsPageHeadingByDefault = false;

        public virtual TagBuilder GenerateTprAddressLookup(bool isLegendPageHeading, AttributeDictionary? legendAttributes, IHtmlContent? legendContent, IHtmlContent? childContent, string? fieldsetDescribedBy)
        {
            var container = new TagBuilder(TprAddressLookupElement);
            container.AddCssClass("tpr-address-lookup");

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

                if (isLegendPageHeading)
                {
                    var h1 = new TagBuilder("h1");
                    h1.MergeCssClass("govuk-fieldset__heading");
                    h1.InnerHtml.AppendHtml(legendContent);
                    legendTag.InnerHtml.AppendHtml(h1);
                }
                else
                {
                    legendTag.InnerHtml.AppendHtml(legendContent);
                    legendTag.MergeCssClass("govuk-fieldset__legend--for-fieldset");
                }
                               

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
