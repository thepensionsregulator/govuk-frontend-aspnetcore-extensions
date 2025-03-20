using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using System.Collections.Generic;


namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprHeaderSearchTagHelper : TagHelper
    {
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal const string TagName = "tpr-header-search";

        public TprHeaderSearchTagHelper()
          : this(htmlGenerator: null)
        {
        }

        internal TprHeaderSearchTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerSearchContext = new TprHeaderSearchContext();

            using (context.SetScopedContextItem(headerSearchContext))
            {
                await output.GetChildContentAsync();
            }

            var headerSearch = new TprHeaderSearch()
            {
                SearchAttributes = headerSearchContext.SearchAttributes,
                SearchBoxPrompt = headerSearchContext.SearchBoxPrompt,
                SearchBoxAllowHtml = headerSearchContext.SearchBoxAllowHtml
            };

            var tagBuilder = _htmlGenerator.GenerateTprHeaderSearch(headerSearch);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
