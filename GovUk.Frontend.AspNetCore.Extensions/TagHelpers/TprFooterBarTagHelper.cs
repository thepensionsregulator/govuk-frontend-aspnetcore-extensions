using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Generates a TPR footer bar component
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.TprFooterBarElement)]
    public class TprFooterBarTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar";

        private const string LogoAltAttributeName = "logo-alt";
        private const string LogoHrefAttributeName = "logo-href";
        private const string CopyrightAttributeName = "copyright";

        private string _logoAlt = ComponentGenerator.FooterLogoDefaultAlt;

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprFooterBarTagHelper"/>.
        /// </summary>
        public TprFooterBarTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprFooterBarTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <summary>
        /// The <c>alt</c> attribute for the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoAltAttributeName)]
        public string LogoAlt
        {
            get => _logoAlt;
            set => _logoAlt = Guard.ArgumentNotNullOrEmpty(nameof(value), value);
        }

        /// <summary>
        /// The <c>href</c> attribute for the link around the TPR logo.
        /// </summary>
        [HtmlAttributeName(LogoHrefAttributeName)]
        public string? LogoHref { get; set; } = ComponentGenerator.HeaderLogoDefaultHref;

        /// <summary>
        /// The <c>href</c> attribute for the link around the TPR logo.
        /// </summary>
        [HtmlAttributeName(CopyrightAttributeName)]
        public string Copyright { get; set; } = ComponentGenerator.CopyrightDefaultContent;

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            IHtmlContent? content = null;

            if (output.TagMode == TagMode.StartTagAndEndTag)
            {
                content = await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTprFooterBar(LogoHref, LogoAlt, content, Copyright, output.Attributes.ToAttributeDictionary());

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
