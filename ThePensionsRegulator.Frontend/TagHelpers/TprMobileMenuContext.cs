using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuContext
    {
        public AttributeDictionary? Attributes { get; set; }

        private readonly List<TprMobileMenuItemsContext> _menuItems = new();

        public IReadOnlyList<TprMobileMenuItemsContext> MenuItems => _menuItems;

        public void AddItem(TprMobileMenuItemsContext item)
        {
            _menuItems.Add(item);
        }
    }
}
