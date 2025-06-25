using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprMobileMenuTagHelper : TagHelper
    {

        internal const string TagName = "tpr-mobile-menu";
       
        private const string AriaLabelName = "aria-label";

        [HtmlAttributeName(AriaLabelName)]
        public string? AriaLabel { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var mobileMenuContext = new TprMobileMenuContext();
            mobileMenuContext.MobileMenuAriaLabel = AriaLabel;

            if (output.Attributes != null)
            {
                mobileMenuContext.Attributes = output.Attributes.ToAttributeDictionary();
            }

            using (context.SetScopedContextItem(mobileMenuContext))
            {
                await output.GetChildContentAsync();
            }

            var headerBarContext = context.GetContextItem<TprHeaderBarContext>();

            headerBarContext.SetMobileMenu(mobileMenuContext);
            output.SuppressOutput();
        }
    }
}
