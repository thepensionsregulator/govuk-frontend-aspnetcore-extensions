using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentTagHelper.TagName)]
    public class TprDocumentTitleTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document-title";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = (TprDocumentContext)context.Items[typeof(TprDocumentsTagHelper)];
            documentContext.DocumentTitle = await output.GetChildContentAsync();

            output.PreElement.SetHtmlContent("<dt>");
            output.TagName = $"a href='{documentContext.Href}'";

            if (documentContext.Href!.EndsWith(".pdf"))
            {
                output.PostContent.SetHtmlContent($"<br><span class='pdf fileicon'>PDF</span> {documentContext.KbSize}KB, {documentContext.Pages} pages </dt>");
            }
            else if (documentContext.Href!.EndsWith(".docx"))
            {
                output.PostContent.SetHtmlContent($"<br><span class='doc fileicon'>WORD</span> {documentContext.KbSize}KB, {documentContext.Pages} pages </dt>");
            }
            else
            {
                output.PostElement.SetHtmlContent("</dt>");
            }
        }
    }
}