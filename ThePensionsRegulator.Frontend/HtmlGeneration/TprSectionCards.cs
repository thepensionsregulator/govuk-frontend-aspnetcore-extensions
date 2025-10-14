using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprSectionCards
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprSectionCard> Cards { get; set; } = new();
        public string? NewTabText { get; set; }
        public int sectionCardsTitleHeadingLevel = 2;
        public string sectionCardsTitleHeadingClass = "govuk-heading-m";
    }
}
