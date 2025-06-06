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

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuContext = context.GetContextItem<TprMobileMenuItemsContext>();
            var mobileMenuItemContext = new TprMobileMenuItemsSubItemsContext
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
            };

            using (context.SetScopedContextItem(mobileMenuItemContext))
            {
                await output.GetChildContentAsync();
            }

            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
