using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    [HtmlTargetElement(TagName, ParentTag = TprMobileMenuTagHelper.TagName)]
    public class TprMobileMenuItemTagHelper : TagHelper
    {
        internal const string TagName = "tpr-mobile-menu-item";

        private const string UrlAttribute = "url";

        [HtmlAttributeName(UrlAttribute)]
        public string? Url {  get; set; }

        public string? LinkText { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuContext = context.GetContextItem<TprMobileMenuContext>();
            var mobileMenuItemContext = new TprMobileMenuItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                
            };

            using (context.SetScopedContextItem(mobileMenuItemContext))
            {
                await output.GetChildContentAsync();
                             
            }
            
            mobileMenuItemContext.SetItem(mobileMenuItemContext.Attributes, LinkText, Url, mobileMenuItemContext.Placement, mobileMenuItemContext.Hierarchy);
            mobileMenuContext.AddItem(mobileMenuItemContext);
            

            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
