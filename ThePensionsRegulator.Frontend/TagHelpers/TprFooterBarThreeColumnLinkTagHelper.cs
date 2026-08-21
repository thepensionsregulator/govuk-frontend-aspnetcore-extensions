using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprFooterBarThreeColumnLinkTagHelper : TagHelper
    {
        internal const string TagName = "tpr-footer-bar-three-column-link";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttributeName = "link-text";

        [HtmlAttributeName(UrlAttributeName)]
        public required string Url { get; set; }

        [HtmlAttributeName(LinkTextAttributeName)]
        public required string LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var threeColumnLinksContext = context.GetContextItem<TprFooterBarThreeColumnLinksContext>();

            var threeColumnLinkContext = new TprFooterBarThreeColumnLinkContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
            };

            using (context.SetScopedContextItem(threeColumnLinkContext))
            {
                await output.GetChildContentAsync();
            }

            threeColumnLinkContext.SetThreeColumnLink(threeColumnLinkContext.Attributes, LinkText, Url);
            threeColumnLinksContext.AddLink(threeColumnLinkContext);

            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
