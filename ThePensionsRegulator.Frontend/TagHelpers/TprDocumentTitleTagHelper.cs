using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
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
            output.TagName = $"a class=\"govuk-link\" href=\"{documentContext.Href}\"";

            if (documentContext.Href != null)
            {
                if (documentContext.Href.EndsWith(".pdf"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"pdf fileicon\">PDF</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                }
                else if (documentContext.Href.EndsWith(".docx"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">Word</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                }
                else if (documentContext.Href.EndsWith(".dotx"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">DOTX</span> {documentContext.KbSize}KB </dt>");
                }
                else if (documentContext.Href.EndsWith(".pptx"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"powerpoint fileicon\">PPTX</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                }
                else if (documentContext.Href.EndsWith(".xlsx"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"excel fileicon\">XLSX</span> {documentContext.KbSize}KB </dt>");
                }
                else if (documentContext.Href.EndsWith(".xlst"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"excel fileicon\">XLST</span> {documentContext.KbSize}KB </dt>");
                }
                else if (documentContext.Href.EndsWith(".csv"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"excel fileicon\">CSV</span> {documentContext.KbSize}KB </dt>");
                }
                else
                {
                    output.PostElement.SetHtmlContent("</dt>");
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(documentContext.Href), "Document href cannot be null");
            }
        }
    }
}