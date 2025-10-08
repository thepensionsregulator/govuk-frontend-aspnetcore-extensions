using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprGlobalNavigationService : ITprGlobalNavigationService
    {
        public IList<TprHeaderMenuParentItem>? GetMenuItems(IContent settingsNode, string propertyAlias, string linkTextAlias, string linkUrlAlias, string childAlias)
        {

            var headerMenuBlockList = settingsNode.GetValue(propertyAlias)?.ToString();
            if (headerMenuBlockList == null) { throw new ArgumentNullException($"No block list with alias `{propertyAlias}` could be found in {settingsNode}"); }

            var blockListJson = JsonConvert.DeserializeObject<BlockValue>(headerMenuBlockList);
            if (blockListJson == null) { throw new JsonException($"An error has occured reading {headerMenuBlockList}"); }

            IList<TprHeaderMenuParentItem> menuItems = [];
            IList<TprHeaderMenuChildItem>? childMenuItems = [];

            foreach (var item in blockListJson.ContentData)
            {
                var linkText = item.RawPropertyValues[linkTextAlias]?.ToString();

                var linkUrlJson = item.RawPropertyValues[linkUrlAlias]?.ToString();
                var linkUrl = GetUrlFromJson(linkUrlJson);

                var childItems = item.RawPropertyValues[childAlias]?.ToString();
                if (childItems != null)
                {
                    var children = JsonConvert.DeserializeObject<BlockValue>(childItems);
                    if (children != null)
                    {
                        foreach (var childItem in children.ContentData)
                        {
                            var childLinkText = childItem.RawPropertyValues[linkTextAlias]?.ToString();

                            var childLinkUrlJson = childItem.RawPropertyValues[linkUrlAlias]?.ToString();

                            var childLinkUrl = GetUrlFromJson(childLinkUrlJson);

                            childMenuItems.Add(new TprHeaderMenuChildItem(childLinkText, childLinkUrl));
                        }
                    }
                }
                var newItem = new TprHeaderMenuParentItem(linkText, linkUrl, childMenuItems);

                menuItems.Add(newItem);
            }

            return menuItems;
        }

        public string? GetUrlFromJson(string? json)
        {
            if(json == null) { return null; }

            var data = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
            string? linkUrl = null;

            if (data != null && data.Count > 0)
            {
                var item = data[0];
                if (item.ContainsKey("url"))
                {
                    linkUrl = item["url"];
                }
                else if (item.ContainsKey("udi"))
                {
                    linkUrl = item["udi"];
                }
            }
            return linkUrl;
        }
    }

    public class TprHeaderMenuViewModel
    {
        public string? TprHeaderMenuBlockListAlias { get; set; }
        public string? LinkTextAlias { get; set; }
        public string? LinkUrlAlias { get; set; }
        public string? ChildMenuItemsAlias { get; set; }
    }
}
