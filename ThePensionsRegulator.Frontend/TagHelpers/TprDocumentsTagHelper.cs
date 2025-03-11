using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprDocumentTagHelper.TagName, TprDocumentTitleTagHelper.TagName, TprDocumentDescriptionTagHelper.TagName)]
    class TprDocumentsTagHelper : TagHelper
    {
        internal const string TagName = "tpr-documents";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "dl";
        }
    }
}
