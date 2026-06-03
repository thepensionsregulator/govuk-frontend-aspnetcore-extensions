using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Represents the label in the TPR header bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprHeaderBarTagHelper.TagName)]

    public class TprHeaderBarLabelTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-bar-label";

        /// <summary>
        /// Gets or sets whether to allow HTML content.
        /// </summary>
        [HtmlAttributeName("allow-html")]
        public bool AllowHtml { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = context.GetContextItem<TprHeaderBarContext>();

            var childContent = await output.GetChildContentAsync();

            barContext.SetLabel(output.Attributes.ToAttributeDictionary(), childContent.Snapshot(), AllowHtml);

            output.SuppressOutput();
        }
    }
}