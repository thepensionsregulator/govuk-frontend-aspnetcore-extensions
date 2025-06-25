using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprMobileMenuContext
    {
        public AttributeDictionary? Attributes { get; set; }

        private readonly List<TprMobileMenuParentItemsContext> _menuParentItems = new();
        public IReadOnlyList<TprMobileMenuParentItemsContext> MenuParentItems => _menuParentItems;
        public string? MobileMenuAriaLabel {  get; set; }
     
        public void AddParentItem(TprMobileMenuParentItemsContext item)
        {
            _menuParentItems.Add(item);
        }
    }
}
