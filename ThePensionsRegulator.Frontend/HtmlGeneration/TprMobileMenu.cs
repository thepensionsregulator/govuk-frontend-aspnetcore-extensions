using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprMobileMenu
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprMobileMenuParentItem>? MobileMenuItems { get; set; } = new();
        public string? AriaLabel { get; set; }
    }
}
