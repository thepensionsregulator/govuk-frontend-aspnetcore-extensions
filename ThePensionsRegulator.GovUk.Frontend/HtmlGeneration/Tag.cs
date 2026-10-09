using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.GovUk.Frontend.HtmlGeneration
{
    public class Tag
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public string Text { get; set; } = string.Empty;
        public string CssClass { get; set; } = string.Empty;
    }
}
