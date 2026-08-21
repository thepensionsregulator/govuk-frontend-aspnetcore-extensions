using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprDocumentTagHelper.TagName)]
    public class TprDocumentDescriptionTagHelper : TagHelper
    {
        internal const string TagName = "tpr-document-description";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = (TprDocumentContext)context.Items[typeof(TprDocumentsTagHelper)];
            var innerText = await output.GetChildContentAsync();
            documentContext.DocumentDescription = innerText;
            var publishedLabel = documentContext.PublishedLabel ?? "Published";

            if (!string.IsNullOrEmpty(documentContext.DatePublished))
            {
                output.PreElement.SetHtmlContent($"<dd>{publishedLabel}: {documentContext.DatePublished}</dd>");
            }

            output.TagName = innerText.IsEmptyOrWhiteSpace ? "" : "dd";
        }
    }
}