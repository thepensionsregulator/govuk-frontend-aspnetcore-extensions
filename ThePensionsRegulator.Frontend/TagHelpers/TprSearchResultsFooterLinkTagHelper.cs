using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprSearchResultsFooterLinksTagHelper.TagName)]
    public class TprSearchResultsFooterLinkTagHelper : TagHelper
    {
        internal const string TagName = "a";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var searchFooterContext = context.GetContextItem<TprSearchFooterLinksContext>();

            var childContent = await output.GetChildContentAsync();

            searchFooterContext.AddLink(output.Attributes.ToAttributeDictionary(), childContent.Snapshot());

            output.SuppressOutput();
        }
    }
}
