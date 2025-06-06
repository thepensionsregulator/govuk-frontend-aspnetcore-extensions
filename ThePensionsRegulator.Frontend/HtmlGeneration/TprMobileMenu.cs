using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprMobileMenu
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprMobileMenuItem>? MobileMenuItems { get; set; } = new();
    }
}
