using Microsoft.AspNetCore.Razor.TagHelpers;
using GovUk.Frontend.AspNetCore.Extensions.TagHelpers;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprDocumentTagHelper.TagName)]
    class TprDocumentsTagHelper : TagHelper
    {
        internal const string TagName = "tpr-documents";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "dl";
        }
    }
}
