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
    [RestrictChildren(TprFooterBarLogoTagHelper.TagName, TprFooterBarCopyrightTagHelper.TagName, TprFooterBarContentTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TprFooterBarElement)]
    public class TprFooterBarTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar";

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

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var barContext = new TprFooterBarContext();

            using (context.SetScopedContextItem(barContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTprFooterBar(new TprFooterBar
            {
                FooterBarAttributes = output.Attributes.ToAttributeDictionary(),
                LogoAttributes = barContext.LogoAttributes,
                LogoHref = barContext.LogoHref ?? ComponentGenerator.FooterLogoDefaultHref,
                LogoAlternativeText = barContext.LogoAlternativeText ?? ComponentGenerator.FooterLogoDefaultAlt,
                CopyrightAttributes = barContext.CopyrightAttributes,
                Copyright = barContext.Copyright ?? new HtmlString(ComponentGenerator.CopyrightDefaultContent),
                CopyrightAllowHtml = barContext.CopyrightAllowHtml,
                ContentAttributes = barContext.ContentAttributes,
                Content = barContext.Content,
                ContentAllowHtml = barContext.ContentAllowHtml
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
