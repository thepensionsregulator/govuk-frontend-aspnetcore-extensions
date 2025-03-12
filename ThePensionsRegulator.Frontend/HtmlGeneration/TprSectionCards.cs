using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprSectionCards
    {
        public AttributeDictionary? ContainerAttributes { get; set; }
        public List<TprSectionCardsCard> Cards { get; set; } = new();
    }
}
