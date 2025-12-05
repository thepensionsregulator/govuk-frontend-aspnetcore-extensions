using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprFooterThreeColumnLink
    {
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText { get; set; }
        public string? LinkUrl { get; set; }
    }
}
