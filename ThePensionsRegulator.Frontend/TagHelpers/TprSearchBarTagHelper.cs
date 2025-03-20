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
    public class TprSearchBarTagHelper : TagHelper
    {
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal const string TagName = "tpr-search-bar";

        public TprSearchBarTagHelper()
          : this(htmlGenerator: null)
        {
        }

        internal TprSearchBarTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var cardsContext = new TprSearchBarContext();

            using (context.SetScopedContextItem(cardsContext))
            {
                await output.GetChildContentAsync();
            }

            var searchBar = new TprSearchBar()
            {
                SearchAttributes = cardsContext.SearchAttributes,
                SearchBoxPrompt = cardsContext.SearchBoxPrompt,
                SearchBoxAllowHtml = cardsContext.SearchBoxAllowHtml
            };

            var tagBuilder = _htmlGenerator.GenerateTprSearchBar(searchBar);

            output.TagName = tagBuilder.TagName;
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Clear();
            output.MergeAttributes(tagBuilder);
            output.Content.SetHtmlContent(tagBuilder.InnerHtml);
        }
    }
}
