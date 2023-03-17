using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the logo in the TPR header bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprHeaderBarTagHelper.TagName, TagStructure = TagStructure.WithoutEndTag)]

    public class TprHeaderBarLogoTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-bar-logo";

        private const string LogoAltAttributeName = "alt";
        private const string LogoHrefAttributeName = "href";

        /// <summary>
        /// The <c>alt</c> attribute for the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoAltAttributeName)]
        public string AlternativeText { get; set; } = ComponentGenerator.HeaderLogoDefaultAlt;

        /// <summary>
        /// The <c>href</c> attribute for the link around the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoHrefAttributeName)]
        public string? Href { get; set; } = ComponentGenerator.HeaderLogoDefaultHref;

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = context.GetContextItem<TprHeaderBarContext>();

            barContext.SetLogo(output.Attributes.ToAttributeDictionary(), Href, AlternativeText);

            output.SuppressOutput();
        }
    }
}