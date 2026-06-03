using GovUk.Frontend.AspNetCore;
using GovUk.Frontend.AspNetCore.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprHeaderMenuTagHelper : TagHelper
    {

        internal const string TagName = "tpr-header-menu";
       
        private const string MenuAriaLabelName = "aria-label";
        private const string MenuItemAriaLabelName = "menu-item-aria-label";
        private const string NoJsNavPageName = "no-js-destination";
        private const string ToggleClosedName = "close-label";
        private const string ToggleOpenName = "open-label";

        [HtmlAttributeName(MenuAriaLabelName)]
        public string? MenuAriaLabel { get; set; }

        [HtmlAttributeName(MenuItemAriaLabelName)]
        public string? MenuItemAriaLabel { get;set; }

        [HtmlAttributeName(NoJsNavPageName)]
        public string? NoJsNavPage { get; set; }

        [HtmlAttributeName(ToggleClosedName)]
        public string? ToggleClosed { get; set; }

        [HtmlAttributeName(ToggleOpenName)]
        public string? ToggleOpen { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var headerMenuContext = new TprHeaderMenuContext();
            headerMenuContext.HeaderMenuAriaLabel = MenuAriaLabel;
            headerMenuContext.HeaderMenuItemAriaLabel = MenuItemAriaLabel;
            headerMenuContext.MobileMenuNoJsNavPage = NoJsNavPage;
            headerMenuContext.HeaderMenuToggleClosed = ToggleClosed;
            headerMenuContext.HeaderMenuToggleOpen = ToggleOpen;

            if (output.Attributes != null)
            {
                headerMenuContext.Attributes = output.Attributes.ToAttributeDictionary();
            }

            using (context.SetScopedContextItem(headerMenuContext))
            {
                await output.GetChildContentAsync();
            }

            var headerBarContext = context.GetContextItem<TprHeaderBarContext>();

            headerBarContext.SetHeaderMenu(headerMenuContext);
            output.SuppressOutput();
        }
    }
}
