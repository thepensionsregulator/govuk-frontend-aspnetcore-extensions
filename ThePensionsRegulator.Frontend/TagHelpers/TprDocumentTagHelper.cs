using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentsTagHelper.TagName)]
    [RestrictChildren(TprDocumentTitleTagHelper.TagName, TprDocumentDescriptionTagHelper.TagName)]
    public class TprDocumentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document";

        [HtmlAttributeName("href")]
        public required string Href { get; set; }

        [HtmlAttributeName("kbsize")]
        public string? KbSize { get; set; }

        [HtmlAttributeName("pages")]
        public string? Pages { get; set; }

        [HtmlAttributeName("datepublished")]
        public string? DatePublished { get; set; }

        [HtmlAttributeName("description")]
        public string? Description { get; set; }

        [HtmlAttributeName("innercss")]
        public string? InnerCss { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (string.IsNullOrEmpty(Href)) { throw new ArgumentNullException(nameof(Href), "Document href cannot be null"); }

            var documentContext = (TprDocumentContext)context.Items[typeof(TprDocumentsTagHelper)];
            documentContext.Href = Href;
            documentContext.KbSize = KbSize;
            documentContext.Pages = Pages;
            documentContext.DatePublished = DatePublished;
            documentContext.Description = Description;
            documentContext.Document = await output.GetChildContentAsync();

            output.TagName = $"div class=\"{TagName} {InnerCss}\"";
        }
    }
}