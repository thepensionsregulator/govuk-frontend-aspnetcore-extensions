using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSearchTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search";

        private readonly ITprHtmlGenerator _htmlGenerator;

        [HtmlAttributeName("faq-api-url")]
        public string FaqApiUrl { get; set; } = string.Empty;

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
            var tagBuilder = _htmlGenerator.GenerateTprSearch(FaqApiUrl);
            
            var result = await output.GetChildContentAsync();
            tagBuilder.InnerHtml.SetHtmlContent(result);
            output.TagName = tagBuilder.TagName;
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
