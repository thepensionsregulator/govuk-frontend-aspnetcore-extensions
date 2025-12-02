using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprGlobalNavigationService : ITprGlobalNavigationService
    {
        public IList<TprHeaderMenuItem> GetMenuItems(IPublishedContent settingsNode, TprHeaderMenuViewModel tprHeaderMenuViewModel)
        {
            if (tprHeaderMenuViewModel == null) { return new List<TprHeaderMenuItem>(); }

            var headerMenuBlockList = settingsNode?.Value<OverridableBlockListModel>(tprHeaderMenuViewModel.MenuBlockListAlias);
            if (headerMenuBlockList == null) { return new List<TprHeaderMenuItem>(); }

            IList<TprHeaderMenuItem> menuItems = [];

            foreach (var item in headerMenuBlockList)
            {
                
                var linkUrl = item?.Content.Value<Link>(tprHeaderMenuViewModel.LinkUrlAlias);
                var linkTextValue = item?.Content.Value<string>(tprHeaderMenuViewModel.LinkTextAlias);
                var linkText = string.IsNullOrWhiteSpace(linkTextValue) ? linkUrl?.Name : linkTextValue; 

                IList<TprHeaderMenuChildItem>? childMenuItems = [];

                var headerMenuChildItems = item?.Content.Value<OverridableBlockListModel>(tprHeaderMenuViewModel.MenuItemsChildAlias);
                if (headerMenuChildItems != null)
                {
                    foreach (var child in headerMenuChildItems)
                    {                      
                        var childLinkUrl = child?.Content.Value<Link>(tprHeaderMenuViewModel.LinkUrlAlias);
                        var childLinkTextValue = child?.Content.Value<string>(tprHeaderMenuViewModel.LinkTextAlias);
                        var childLinkText = string.IsNullOrWhiteSpace(childLinkTextValue) ? childLinkUrl?.Name : childLinkTextValue;

                        if (childLinkText != null && childLinkUrl?.Url != null)
                        {
                            childMenuItems.Add(new TprHeaderMenuChildItem(childLinkText.ToFirstUpper(), childLinkUrl.Url));
                        }
                    }
                }
                if (linkText != null && linkUrl?.Url != null)
                {
                    var newItem = new TprHeaderMenuItem(linkText.ToFirstUpper(), linkUrl.Url, childMenuItems);
                    menuItems.Add(newItem);
                }
            }

            return menuItems;
        }
    }
}
