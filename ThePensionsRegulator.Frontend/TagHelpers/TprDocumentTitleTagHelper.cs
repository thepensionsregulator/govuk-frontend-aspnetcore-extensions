using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentTagHelper.TagName)]
    class TprDocumentTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document-title";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = context.GetContextItem<TprDocumentContext>();
            
            output.PreElement.SetHtmlContent("<dt>");
            output.TagName = "a";

            if (documentContext.Href!.EndsWith(".pdf"))
            {
                output.PostContent.SetHtmlContent($"/n<span class='pdf fileicon'>PDF</span> {documentContext.KbSize}KB, {documentContext.Pages} pages");
            }
            if (documentContext.Href!.EndsWith(".docx"))
            {
                output.PostContent.SetHtmlContent($"/n<span class='doc fileicon'>WORD</span> {documentContext.KbSize}KB, {documentContext.Pages} pages");
            }

            output.PostElement.SetHtmlContent("</dt>");
        }
    }
}