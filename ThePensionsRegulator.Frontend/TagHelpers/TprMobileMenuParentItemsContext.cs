using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuParentItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkDestination)? _menuIParentItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuIParentItem?.LinkText;
        public string? LinkDestination => _menuIParentItem?.LinkDestination;

        private readonly List<TprMobileMenuChildItemsContext> _subMenuItems = new();
        public IReadOnlyList<TprMobileMenuChildItemsContext> SubMenuItems => _subMenuItems;

        public void SetParentItem(AttributeDictionary attributes, string? linkText, string? linkDestination)
        {
            _menuIParentItem = (attributes, linkText, linkDestination);
        }

        public void AddChildItem(TprMobileMenuChildItemsContext tprMobileMenuItemsSubItemsContext)
        {
            _subMenuItems.Add(tprMobileMenuItemsSubItemsContext);
        }
    }
}
