using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuParentItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkDestination)? _menuIParentItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuIParentItem?.LinkText;
        public string? LinkDestination => _menuIParentItem?.LinkDestination;

        private readonly List<TprHeaderMenuChildItemsContext> _headerMenuChildItems = new();
        public IReadOnlyList<TprHeaderMenuChildItemsContext> HeaderMenuChildItems => _headerMenuChildItems;

        public void SetParentItem(AttributeDictionary attributes, string? linkText, string? linkDestination)
        {
            _menuIParentItem = (attributes, linkText, linkDestination);
        }

        public void AddChildItem(TprHeaderMenuChildItemsContext tprMobileMenuItemsSubItemsContext)
        {
            _headerMenuChildItems.Add(tprMobileMenuItemsSubItemsContext);
        }
    }
}
