using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprRelatedLinksHeadingTagHelper.TagName, TprRelatedLinkTagHelper.TagName)]
    public class TprRelatedLinksTagHelper : TagHelper
    {
        internal const string TagName = "tpr-related-links";

        private readonly ITprHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TprContextBarTagHelper"/>.
        /// </summary>
        public TprRelatedLinksTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprRelatedLinksTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var relatedLinksContext = new TprRelatedLinksContext();

            using (context.SetScopedContextItem(relatedLinksContext))
            {
                await output.GetChildContentAsync();
            }

            var tagBuilder = _htmlGenerator.GenerateTprRelatedLinks(new TprRelatedLinks
            {
                RelatedLinksAttributes = output.Attributes.ToAttributeDictionary(),
                HeadingAttributes = relatedLinksContext.HeadingAttributes,
                HeadingContent = relatedLinksContext.HeadingContent,
                Links = relatedLinksContext.Links
            });

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
