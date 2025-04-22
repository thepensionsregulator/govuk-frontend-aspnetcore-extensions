using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentsTagHelper.TagName)]
    [RestrictChildren(TprDocumentTitleTagHelper.TagName, TprDocumentDescriptionTagHelper.TagName)]
    public class TprDocumentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document";

        [HtmlAttributeName("href")]
        public string? Href { get; set; }

        [HtmlAttributeName("kbsize")]
        public string? KbSize { get; set; }

        [HtmlAttributeName("pages")]
        public string? Pages { get; set; }

        [HtmlAttributeName("datepublished")]
        public string? DatePublished { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = (TprDocumentContext)context.Items[typeof(TprDocumentsTagHelper)];
            documentContext.Href = Href;
            documentContext.KbSize = KbSize;
            documentContext.Pages = Pages;
            documentContext.DatePublished = DatePublished;
            documentContext.Document = await output.GetChildContentAsync();

            output.TagName = $"div class=\"{TagName}\"";
        }
    }
}