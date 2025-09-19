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

                string[] knownFileExtensions = ["csv", "doc", "docx", "dotx", "odt", "pdf", "pptx", "rtf", "xlst", "xlsx"];
                var fileExtension = string.Empty;

                if (documentContext.Href.Contains('.')) { fileExtension = documentContext.Href.Split(".").Skip(1).ToArray()[0].ToString(); }
                var isMediaLink = knownFileExtensions.Contains(fileExtension);

                if (isMediaLink)
                {
                    output.TagName = $"a class=\"govuk-link\" href=\"{documentContext.Href}\" download";
                }
                else
                {
                    output.TagName = $"a class=\"govuk-link\" href=\"{documentContext.Href}\"";
                }

                if (documentContext.Href.EndsWith(".pdf"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"pdf fileicon\">PDF</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"pdf fileicon\">PDF</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".docx") || documentContext.Href.EndsWith(".doc"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">Word</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">Word</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".dotx"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">DOTX</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"doc fileicon\">DOTX</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".pptx"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"powerpoint fileicon\">PPTX</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"powerpoint fileicon\">PPTX</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".rtf"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"misc fileicon\">RTF</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"misc fileicon\">RTF</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".odt"))
                {
                    if (documentContext.Pages == "0")
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"misc fileicon\">ODT</span> {documentContext.KbSize}KB </dt>");
                    }
                    else
                    {
                        output.PostContent.SetHtmlContent($"<br /><span class=\"misc fileicon\">ODT</span> {documentContext.KbSize}KB, {documentContext.Pages} page(s) </dt>");
                    }
                }
                else if (documentContext.Href.EndsWith(".xlsx"))
                {
                    output.PostContent.SetHtmlContent($"<br /><span class=\"excel fileicon\">Excel</span> {documentContext.KbSize}KB </dt>");
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