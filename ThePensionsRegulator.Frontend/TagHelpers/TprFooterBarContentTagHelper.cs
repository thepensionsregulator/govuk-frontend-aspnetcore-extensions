using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Represents the content area in the TPR footer bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprFooterBarTagHelper.TagName)]

    public class TprFooterBarContentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar-content";

        /// <summary>
        /// Gets or sets whether to allow HTML content.
        /// </summary>
        [HtmlAttributeName("allow-html")]
        public bool AllowHtml { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = context.GetContextItem<TprFooterBarContext>();

            var childContent = await output.GetChildContentAsync();

            barContext.SetContent(output.Attributes.ToAttributeDictionary(), childContent.Snapshot(), AllowHtml);

            output.SuppressOutput();
        }
    }
}