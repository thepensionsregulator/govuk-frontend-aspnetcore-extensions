using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the logo in the TPR footer bar component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TprFooterBarTagHelper.TagName, TagStructure = TagStructure.WithoutEndTag)]

    public class TprFooterBarLogoTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar-logo";

        private const string LogoAltAttributeName = "alt";
        private const string LogoHrefAttributeName = "href";

        /// <summary>
        /// The <c>alt</c> attribute for the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoAltAttributeName)]
        public string AlternativeText { get; set; } = ComponentGenerator.FooterLogoDefaultAlt;

        /// <summary>
        /// The <c>href</c> attribute for the link around the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoHrefAttributeName)]
        public string? Href { get; set; } = ComponentGenerator.FooterLogoDefaultHref;

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = context.GetContextItem<TprFooterBarContext>();

            barContext.SetLogo(output.Attributes.ToAttributeDictionary(), Href, AlternativeText);

            output.SuppressOutput();
        }
    }
}