using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuContext
    {
        public AttributeDictionary? Attributes { get; set; }

        private readonly List<TprHeaderMenuItemContext> _headerMenuParentItems = new();
        public IReadOnlyList<TprHeaderMenuItemContext> HeaderMenuParentItems => _headerMenuParentItems;
        public string? HeaderMenuAriaLabel {  get; set; }
        public string? HeaderMenuItemAriaLabel { get; set; }
        public string? MobileMenuNoJsNavPage {  get; set; }
        public string? HeaderMenuToggleOpen {  get; set; }
        public string? HeaderMenuToggleClosed {  get; set; }
     
        public void AddMenuItem(TprHeaderMenuItemContext item)
        {
            _headerMenuParentItems.Add(item);
        }
    }
}
