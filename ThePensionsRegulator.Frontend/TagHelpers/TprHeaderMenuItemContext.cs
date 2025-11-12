using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuItemContext
    {
        private (AttributeDictionary Attributes, string LinkText, string LinkUrl) _menuItem;
        public AttributeDictionary? Attributes { get; set; }
        public string LinkText => _menuItem.LinkText;
        public string LinkUrl => _menuItem.LinkUrl;

        private readonly List<TprHeaderMenuChildItemContext> _headerMenuChildItems = new();
        public IReadOnlyList<TprHeaderMenuChildItemContext> HeaderMenuChildItems => _headerMenuChildItems;

        public void SetMenuItem(AttributeDictionary attributes, string linkText, string linkUrl)
        {
            _menuItem = (attributes, linkText, linkUrl);
        }

        public void AddChildItem(TprHeaderMenuChildItemContext tprMobileMenuItemsSubItemsContext)
        {
            _headerMenuChildItems.Add(tprMobileMenuItemsSubItemsContext);
        }
    }
}
