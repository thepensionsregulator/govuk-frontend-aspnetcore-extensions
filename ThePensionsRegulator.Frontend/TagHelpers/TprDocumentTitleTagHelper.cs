using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Linq;
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

            if (documentContext.Href != null)
            {
                output.PreElement.SetHtmlContent("<dt>");

                string[] knownFileExtensions = [".csv", ".doc", ".docx", ".dotx", ".odt", ".pdf", ".pptx", ".rtf", ".xlst", ".xlsx"];
                var fileExtension = System.IO.Path.GetExtension(documentContext.Href);
                var isMediaLink = knownFileExtensions.Contains(fileExtension);

                output.TagName = $"a class=\"govuk-link\" href=\"{documentContext.Href}\"";

                if (isMediaLink) { output.TagName += "download"; }

                switch (fileExtension)
                {
                    case ".pdf":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("pdf", "PDF", documentContext, includePages: true));
                        break;
                    case ".doc":
                    case ".docx":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("doc", "Word", documentContext, includePages: true));
                        break;
                    case "dotx":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("doc", "DOTX", documentContext, includePages: true));
                        break;
                    case ".pptx":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("powerpoint", "PPTX", documentContext, includePages: true));
                        break;
                    case ".rtf":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("misc", "RTF", documentContext, includePages: true));
                        break;
                    case ".odt":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("misc", "ODT", documentContext, includePages: true));
                        break;
                    case ".xlsx":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("excel", "Excel", documentContext));
                        break;
                    case ".xlst":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("excel", "XLST", documentContext));
                        break;
                    case ".csv":
                        output.PostContent.SetHtmlContent(GenerateFileInfoHtml("excel", "CSV", documentContext));
                        break;
                    default:
                        break;
                }
                output.PostContent.AppendHtml("</dt>");
            }
            else
            {
                throw new ArgumentNullException(nameof(documentContext.Href), "Document href cannot be null");
            }
        }

        private static string GenerateFileInfoHtml(string cssClass, string fileType, TprDocumentContext context, bool includePages = false)
        {
            var html = $"<br /><span class=\"{cssClass} fileicon\">{fileType}</span> {context.KbSize}KB";

            if(includePages && context.Pages != "0")
            {
                html = html + $", {context.Pages} page(s)";
            }

            return html;
        }
    }
}