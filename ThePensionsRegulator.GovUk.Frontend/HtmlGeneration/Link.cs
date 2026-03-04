using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.GovUk.Frontend.HtmlGeneration
{
    public class Link
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public required string Href { get; set; }
    }
}
