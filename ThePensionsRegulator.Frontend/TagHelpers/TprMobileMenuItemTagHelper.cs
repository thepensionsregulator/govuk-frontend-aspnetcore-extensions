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

            output.TagName = TagName;
            output.SuppressOutput();
        }
    }
}
