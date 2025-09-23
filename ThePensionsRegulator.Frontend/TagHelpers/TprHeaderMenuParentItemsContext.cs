using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuParentItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkUrl)? _menuIParentItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuIParentItem?.LinkText;
        public string? LinkUrl => _menuIParentItem?.LinkUrl;

        private readonly List<TprHeaderMenuChildItemsContext> _headerMenuChildItems = new();
        public IReadOnlyList<TprHeaderMenuChildItemsContext> HeaderMenuChildItems => _headerMenuChildItems;

        public void SetParentItem(AttributeDictionary attributes, string? linkText, string? linkUrl)
        {
            _menuIParentItem = (attributes, linkText, linkUrl);
        }

        public void AddChildItem(TprHeaderMenuChildItemsContext tprMobileMenuItemsSubItemsContext)
        {
            _headerMenuChildItems.Add(tprMobileMenuItemsSubItemsContext);
        }
    }
}
