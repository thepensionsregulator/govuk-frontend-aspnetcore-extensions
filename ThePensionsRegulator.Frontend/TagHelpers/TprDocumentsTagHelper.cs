using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TprDocumentTagHelper.TagName)]
    public class TprDocumentsTagHelper : TagHelper
    {
        internal const string TagName = "tpr-documents";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var documentContext = new TprDocumentContext();
            context.Items.Add(typeof(TprDocumentsTagHelper), documentContext);

            await output.GetChildContentAsync();

            output.TagName = $"dl class=\"{TagName} govuk-list\"";
        }
    }
}
