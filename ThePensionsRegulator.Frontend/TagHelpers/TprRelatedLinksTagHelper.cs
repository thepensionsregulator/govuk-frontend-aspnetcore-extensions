using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using GovUk.Frontend.AspNetCore.Extensions.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprRelatedLinksHeadingTagHelper.TagName, TprRelatedLinkTagHelper.TagName)]
    public class TprRelatedLinksTagHelper : TagHelper
    {
        internal const string TagName = "tpr-related-links";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.SetHtmlContent($"<div class='{TagName} govuk-body'>");
            output.TagName = "ul";
            output.PostElement.SetHtmlContent("</div>");
        }
    }
}
