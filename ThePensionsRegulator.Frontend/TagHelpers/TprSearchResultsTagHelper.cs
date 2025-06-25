using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    public class TprSearchResultsTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search-results";

        private readonly ITprHtmlGenerator _htmlGenerator;

        [HtmlAttributeName("popular-content-url")]
        public string PopularContentApiUrl { get; set; } = string.Empty;

        [HtmlAttributeName("search-content-url")]
        public string SearchContentApiUrl { get; set; } = string.Empty;

        [HtmlAttributeName("content-by-id-url")]
        public string ContentByIdApiUrl { get; set; } = string.Empty;

        internal TprSearchResultsTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprSearchResultsTagHelper() : this(null)
        {

        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = _htmlGenerator.GenerateTprSearchResults(PopularContentApiUrl, SearchContentApiUrl, ContentByIdApiUrl);

            var result = await output.GetChildContentAsync();
            tagBuilder.InnerHtml.SetHtmlContent(result);
            output.TagName = tagBuilder.TagName;
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
