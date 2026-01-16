using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TprAddressLookupElement = "fieldset";

        public virtual TagBuilder GenerateTprAddressLookup(AttributeDictionary? legendAttributes, IHtmlContent? legend, IHtmlContent? childContent)
        {
            var fieldSet = new TagBuilder(TprAddressLookupElement);
            fieldSet.AddCssClass("govuk-fieldset tpr-address-lookup");

            if (legend is not null)
            {
                var legendTag = new TagBuilder("legend");
                legendTag.AddCssClass("govuk-fieldset__legend govuk-fieldset__legend--for-field");

                if (legendAttributes is not null)
                {
                    legendTag.MergeAttributes(legendAttributes);
                }

                legendTag.InnerHtml.AppendHtml(legend);
                fieldSet.InnerHtml.AppendHtml(legendTag);
            }

            if (childContent is not null)
            {
                fieldSet.InnerHtml.AppendHtml(childContent);
            }

            return fieldSet;
        }
    }
}
