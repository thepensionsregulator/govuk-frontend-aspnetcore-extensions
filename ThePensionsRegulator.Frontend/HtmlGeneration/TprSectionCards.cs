using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprSectionCards
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprSectionCard> Cards { get; set; } = new();
    }
}
