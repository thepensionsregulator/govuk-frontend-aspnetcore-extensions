using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSearchTagHelper.TagName)]
    public class TprSearchFooterLinksTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search-footer-links";
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal TprSearchFooterLinksTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprSearchFooterLinksTagHelper() : this(null) { }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var footerLinksContext = new TprSearchFooterLinksContext();

            using (context.SetScopedContextItem(footerLinksContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTprSearchFooterLinks(new TprSearchFooterLinks
            {
                Links = footerLinksContext.Links
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
