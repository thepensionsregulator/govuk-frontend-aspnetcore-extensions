using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentTagHelper.TagName)]
    class TprDocumentDescriptionTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document-description";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "dd";
        }
    }
}