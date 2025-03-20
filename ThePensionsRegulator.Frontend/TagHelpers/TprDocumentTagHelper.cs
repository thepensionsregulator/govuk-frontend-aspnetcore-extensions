using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentsTagHelper.TagName)]
    [RestrictChildren(TprDocumentTitleTagHelper.TagName, TprDocumentDescriptionTagHelper.TagName)]
    class TprDocumentTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document";

        [HtmlAttributeName("href")]
        public string? Href { get; set; }

        [HtmlAttributeName("kbsize")]
        public string? KbSize { get; set; }

        [HtmlAttributeName("pages")]
        public string? Pages { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = new TprDocumentContext();
            context.Items.Add(typeof(TprDocumentTagHelper), documentContext);

            output.TagName = "div";
        }
    }
}