using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprHeaderMenuTagHelper.TagName)]
    public class TprHeaderMenuParentItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-header-menu-parent-item";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttributeName = "link-text";

        [HtmlAttributeName(UrlAttributeName)]
        public string? Url {  get; set; }

        [HtmlAttributeName(LinkTextAttributeName)]
        public string? LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerMenuContext = context.GetContextItem<TprHeaderMenuContext>();
            var headerMenuItemContext = new TprHeaderMenuParentItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),               
            };

            using (context.SetScopedContextItem(headerMenuItemContext))
            {
                await output.GetChildContentAsync();                           
            }
            
            headerMenuItemContext.SetParentItem(headerMenuItemContext.Attributes, LinkText, Url);
            headerMenuContext.AddParentItem(headerMenuItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
