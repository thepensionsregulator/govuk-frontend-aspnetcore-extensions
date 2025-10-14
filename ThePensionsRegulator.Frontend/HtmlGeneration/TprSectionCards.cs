using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprSectionCards
    {
        public AttributeDictionary? Attributes { get; set; }
        public List<TprSectionCard> Cards { get; set; } = [];
        public string? NewTabText { get; set; }
        public int SectionCardsTitleHeadingLevel = 2;
        public string SectionCardsTitleHeadingClass = "govuk-heading-m";
        public string? NavigationAriaLabel { get; set; }
    }
}
