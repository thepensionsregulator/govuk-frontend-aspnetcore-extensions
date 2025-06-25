using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprMobileMenuTagHelper.TagName)]
    public class TprMobileMenuParentItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-mobile-menu-parent-item";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttriubuteName = "link-text";

        [HtmlAttributeName(UrlAttributeName)]
        public string? Url {  get; set; }

        [HtmlAttributeName(LinkTextAttriubuteName)]
        public string? LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuContext = context.GetContextItem<TprMobileMenuContext>();
            var mobileMenuItemContext = new TprMobileMenuParentItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),               
            };

            using (context.SetScopedContextItem(mobileMenuItemContext))
            {
                await output.GetChildContentAsync();                           
            }
            
            mobileMenuItemContext.SetParentItem(mobileMenuItemContext.Attributes, LinkText, Url);
            mobileMenuContext.AddParentItem(mobileMenuItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
