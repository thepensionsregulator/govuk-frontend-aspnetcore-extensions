using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the copyright notice in the TPR footer bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprFooterBarTagHelper.TagName)]

    public class TprFooterBarCopyrightTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar-copyright";

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

            barContext.SetCopyright(output.Attributes.ToAttributeDictionary(), childContent.Snapshot(), AllowHtml);

            output.SuppressOutput();
        }
    }
}