using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card-title";

        [HtmlAttributeName("href")]
        public string? Url { get; set; }

        [HtmlAttributeName("target")]
        public string? Target { get; set; }

        /// <summary>
        /// Gets or sets whether to allow HTML content.
        /// </summary>
        [HtmlAttributeName("allow-html")]
        public bool AllowHtml { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardContext>();
            var content = await output.GetChildContentAsync();

            cardContext.SetTitle(output.Attributes.ToAttributeDictionary(),
                content,
                AllowHtml,
                Url,
                Target);

            output.SuppressOutput();
        }
    }
}
