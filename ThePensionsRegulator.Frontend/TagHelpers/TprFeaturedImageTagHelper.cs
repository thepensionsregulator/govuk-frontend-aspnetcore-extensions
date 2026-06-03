using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    /// <summary>
    /// Generates a GOV.UK back link component.
    /// </summary>
    [HtmlTargetElement(TagName)]
    [OutputElementHint(ComponentGenerator.BackToTopLinkElement)]
    public class TprFeaturedImageTagHelper : TagHelper
    {
        internal const string TagName = "tpr-featured-image";

        [HtmlAttributeName("image-url")]
        public string? ImageUrl { get; set; } = string.Empty; 

        [HtmlAttributeName("image-alt")]
        public string? ImageAlt { get; set; } = string.Empty; 

        [HtmlAttributeName("horizontal")]
        public bool? Horizontal { get; set; } = false; 

        [HtmlAttributeName("decorative-image")]
        public bool? DecorativeImage { get; set; } = false; 

        private readonly ITprHtmlGenerator _htmlGenerator;
        public TprFeaturedImageTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TprFeaturedImageTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            HtmlString? htmlContent = null;
            var content = (await output.GetChildContentAsync()).GetContent();
            if (!string.IsNullOrEmpty(content))
            {
                htmlContent = new HtmlString(content);
            }
            var tagBuilder = _htmlGenerator.GenerateTprFeaturedImage(
                output.Attributes.ToAttributeDictionary(),
                ImageUrl,
                ImageAlt,
                htmlContent,
                Horizontal ?? false,
                DecorativeImage ?? false
                );

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
