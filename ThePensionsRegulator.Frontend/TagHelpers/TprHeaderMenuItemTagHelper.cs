using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ThePensionsRegulator.GovUk.Frontend;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprHeaderMenuTagHelper.TagName)]
    public class TprHeaderMenuItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-menu-item";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttributeName = "link-text";

        [HtmlAttributeName(UrlAttributeName)]
        public required string Url {  get; set; }

        [HtmlAttributeName(LinkTextAttributeName)]
        public required string LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerMenuContext = context.GetContextItem<TprHeaderMenuContext>();
            var headerMenuItemContext = new TprHeaderMenuItemContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),               
            };

            using (context.SetScopedContextItem(headerMenuItemContext))
            {
                await output.GetChildContentAsync();                           
            }
            
            headerMenuItemContext.SetMenuItem(headerMenuItemContext.Attributes, LinkText, Url);
            headerMenuContext.AddMenuItem(headerMenuItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
