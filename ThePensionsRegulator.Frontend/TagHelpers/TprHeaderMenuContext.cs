using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprHeaderMenuContext
    {
        public AttributeDictionary? Attributes { get; set; }

        private readonly List<TprHeaderMenuParentItemsContext> _headerMenuParentItems = new();
        public IReadOnlyList<TprHeaderMenuParentItemsContext> HeaderMenuParentItems => _headerMenuParentItems;
        public string? HeaderMenuAriaLabel {  get; set; }
        public string? MobileMenuNoJsNavPage {  get; set; }
     
        public void AddParentItem(TprHeaderMenuParentItemsContext item)
        {
            _headerMenuParentItems.Add(item);
        }
    }
}
