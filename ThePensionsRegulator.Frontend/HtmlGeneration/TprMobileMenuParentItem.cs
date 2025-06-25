using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprMobileMenuParentItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText { get; set; }
        public string? LinkDestination { get; set; }
        public List<TprMobileMenuChildItem> SubMenuItems { get; set; } = new();
    }
}