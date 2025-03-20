using Microsoft.AspNetCore.Razor.TagHelpers;

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