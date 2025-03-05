using GovUk.Frontend.AspNetCore.Extensions.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprRelatedLinksTagHelper.TagName)]
    public class TprRelatedLinksHeadingTagHelper : TagHelper
    {
        internal const string TagName = "tpr-related-links-heading";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.SetHtmlContent("<li>");
            output.TagName = "h2";
            output.PostElement.SetHtmlContent("</li>");
        }
    }
}
