using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprGlobalNavigationService : ITprGlobalNavigationService
    {     
        public IList<TprHeaderMenuItem>? GetMenuItems(IPublishedContent settingsNode, TprHeaderMenuViewModel tprHeaderMenuViewModel)
        {
                    
            var headerMenuBlockList = settingsNode?.Value<OverridableBlockListModel>(tprHeaderMenuViewModel.MenuBlockListAlias);
            if (headerMenuBlockList == null) { return null; }

            var blockListMenuItems = headerMenuBlockList?.Where(i => i.Content.ContentType.Alias == tprHeaderMenuViewModel.MenuItemAlias);

            IList<TprHeaderMenuItem> menuItems = [];
            IList<TprHeaderMenuChildItem>? childMenuItems = [];
            if (headerMenuBlockList != null)
            {
                foreach (var item in headerMenuBlockList)
                {
                    var linkText = item?.Content.Value<string>(tprHeaderMenuViewModel.LinkTextAlias);
                    var linkUrl = item?.Content.Value<Link>(tprHeaderMenuViewModel.LinkUrlAlias);

                    var headerMenuChildItems = item?.Content.Value<OverridableBlockListModel>(tprHeaderMenuViewModel.MenuItemsChildAlias);
                    if (headerMenuChildItems != null)
                    {
                        foreach (var child in headerMenuChildItems)
                        {
                            var childLinkText = child?.Content.Value<string>(tprHeaderMenuViewModel.LinkTextAlias);
                            var childLinkUrl = child?.Content.Value<Link>(tprHeaderMenuViewModel.LinkUrlAlias);

                            childMenuItems.Add(new TprHeaderMenuChildItem(childLinkText, childLinkUrl.Url));
                        }
                    }
                    var newItem = new TprHeaderMenuItem(linkText, linkUrl.Url, childMenuItems);
                    menuItems.Add(newItem);
                }
            }
            return menuItems;
        }
    }

    public class TprHeaderMenuViewModel
    {
        [SetsRequiredMembers]
        public TprHeaderMenuViewModel(string menuAlias, string menuItemAlias, string linkTextAlias, string linkUrlAlias, string menuItemChildAlias)
        {
            MenuBlockListAlias = menuAlias;
            MenuItemAlias = menuItemAlias;
            LinkTextAlias = linkTextAlias;
            LinkUrlAlias = linkUrlAlias;
            MenuItemsChildAlias = menuItemAlias;
        }
        public required string MenuBlockListAlias { get; set; }
        public required string MenuItemAlias { get; set; }
        public required string LinkTextAlias { get; set; }
        public required string LinkUrlAlias { get; set; }
        public required string MenuItemsChildAlias { get; set; }
    }
}
