using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSearchTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search";

        private readonly ITprHtmlGenerator _htmlGenerator;

        internal TprSearchTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprSearchTagHelper() : this(null)
        {
            
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = _htmlGenerator.GenerateTprSearch();
            
            var result = await output.GetChildContentAsync();
            tagBuilder.InnerHtml.SetHtmlContent(result);
            output.TagName = tagBuilder.TagName;
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
