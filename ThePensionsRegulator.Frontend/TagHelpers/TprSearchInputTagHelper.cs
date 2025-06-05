using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThePensionsRegulator.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSearchTagHelper.TagName)]
    public class TprSearchInputTagHelper : TagHelper
    {
        internal const string TagName = "tpr-search-input";
        private readonly ITprHtmlGenerator _htmlGenerator;

        internal TprSearchInputTagHelper(ITprHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        public TprSearchInputTagHelper() : this (null) { }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var inner = await output.GetChildContentAsync();
            var content = inner.GetContent();

            var result = _htmlGenerator.GenerateTprSearchInput(content);

            output.TagName = "div";
            output.Content.SetHtmlContent(result);
        }
    }
}
