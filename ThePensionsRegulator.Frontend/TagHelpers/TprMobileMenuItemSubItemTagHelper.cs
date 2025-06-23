using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprMobileMenuItemTagHelper.TagName)]
    public class TprMobileMenuItemSubItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-mobile-menu-item-sub-item";

        private const string UrlAttribute = "url";
        
        [HtmlAttributeName(UrlAttribute)] 
        public string? Url { get; set; }

        public string? LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuItemContext = context.GetContextItem<TprMobileMenuItemsContext>();
            var mobileMenuSubItemContext = new TprMobileMenuItemsSubItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),              
            };

            using (context.SetScopedContextItem(mobileMenuSubItemContext))
            {
                await output.GetChildContentAsync();
            }
            
            mobileMenuSubItemContext.SetSubItem(mobileMenuSubItemContext.Attributes, LinkText, Url, mobileMenuSubItemContext.Placement, mobileMenuSubItemContext.Hierarchy);
            mobileMenuItemContext.AddSubMenuItem(mobileMenuSubItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
