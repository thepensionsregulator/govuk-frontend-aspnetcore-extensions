using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentTagHelper.TagName)]
    class TprDocumentTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document-title";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.PreElement.SetHtmlContent("<dt>");
            output.TagName = "a";
            
            output.PostContent.SetHtmlContent("/n<span class='pdf fileicon'>PDF</span> 200KB");

            output.PostElement.SetHtmlContent("</dt>");
        }
    }
}