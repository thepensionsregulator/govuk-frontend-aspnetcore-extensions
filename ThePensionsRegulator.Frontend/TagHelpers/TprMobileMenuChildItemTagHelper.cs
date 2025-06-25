using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprMobileMenuParentItemTagHelper.TagName)]
    public class TprMobileMenuChildItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-mobile-menu-child-item";

        private const string UrlAttributeName = "href";
        private const string LinkTextAttributeName = "link-text";
        
        [HtmlAttributeName(UrlAttributeName)] 
        public string? Url { get; set; }

        [HtmlAttributeName(LinkTextAttributeName)]
        public string? LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuItemContext = context.GetContextItem<TprMobileMenuParentItemsContext>();
            var mobileMenuSubItemContext = new TprMobileMenuChildItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),              
            };

            using (context.SetScopedContextItem(mobileMenuSubItemContext))
            {
                await output.GetChildContentAsync();
            }
            
            mobileMenuSubItemContext.SetChildItem(mobileMenuSubItemContext.Attributes, LinkText, Url);
            mobileMenuItemContext.AddChildItem(mobileMenuSubItemContext);
            
            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
