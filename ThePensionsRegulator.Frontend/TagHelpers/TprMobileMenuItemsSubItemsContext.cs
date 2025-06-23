using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuItemsSubItemsContext
    {
        private (AttributeDictionary Attributes, string? LinkText, string? LinkDestination, string? Placement, string? Hierarchy)? _menuSubItem;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _menuSubItem?.LinkText;
        public string? LinkDestination => _menuSubItem?.LinkDestination;
        public string? Placement => _menuSubItem?.Placement;
        public string? Hierarchy => _menuSubItem?.Hierarchy;

        public void SetSubItem(AttributeDictionary attributes, string? linkText, string? linkDestination, string? placement, string? hierarchy)
        {
            _menuSubItem = (attributes, linkText, linkDestination, placement, hierarchy);
        }
    }
}
