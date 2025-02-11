using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprRelatedLinksTagHelper.TagName)]
    public class TprRelatedLinkTagHelper : TagHelper
    {
        internal const string TagName = "tpr-related-link";
        internal string? Href {get; set;}

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.SetHtmlContent("<li>");
            output.TagName = "a";
            output.Attributes.Add("href", Href);
            output.PostElement.SetHtmlContent("</li>");
        }
    }
}
