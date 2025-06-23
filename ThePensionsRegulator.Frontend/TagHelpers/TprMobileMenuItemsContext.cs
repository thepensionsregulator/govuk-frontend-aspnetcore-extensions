using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkDestination, string? Placement, string? Hierarchy)? _menuItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuItem?.LinkText;
        public string? LinkDestination => _menuItem?.LinkDestination;
        public string? Placement => _menuItem?.Placement;
        public string? Hierarchy => _menuItem?.Hierarchy;

        private readonly List<TprMobileMenuItemsSubItemsContext> _subMenuItems = new();
        public IReadOnlyList<TprMobileMenuItemsSubItemsContext> SubMenuItems => _subMenuItems;

        public void SetItem(AttributeDictionary attributes, string? linkText, string? linkDestination, string? placement, string? hierarchy)
        {
            _menuItem = (attributes, linkText, linkDestination, placement, hierarchy);
        }

        public void AddSubMenuItem(TprMobileMenuItemsSubItemsContext tprMobileMenuItemsSubItemsContext)
        {
            _subMenuItems.Add(tprMobileMenuItemsSubItemsContext);
        }
    }
}
