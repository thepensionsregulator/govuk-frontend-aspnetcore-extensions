using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprMobileMenuItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public string? Id { get; set; }
        public string? LinkText { get; set; }
        public string? LinkDestination { get; set; }
        public string? Placement { get; set; }
        public string? Hierarchy { get; set; }
        public List<TprMobileMenuSubMenuItem> SubMenuItems { get; set; } = new();

    }
}
