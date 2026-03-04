using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSectionCardContentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-section-card-content";

        /// <summary>
        /// Gets or sets whether to allow HTML content.
        /// </summary>
        [HtmlAttributeName("allow-html")]
        public bool AllowHtml { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardContext = context.GetContextItem<TprSectionCardContext>();
            var content = await output.GetChildContentAsync();

            cardContext.SetContent(
                output.Attributes.ToAttributeDictionary(),
                content,
                AllowHtml
             );

            output.SuppressOutput();
        }
    }
}
