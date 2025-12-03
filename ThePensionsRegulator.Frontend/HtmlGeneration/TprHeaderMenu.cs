using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenu
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprHeaderMenuItem>? HeaderMenuItems { get; set; } = new();
        public string? AriaLabel { get; set; }
    }
}
