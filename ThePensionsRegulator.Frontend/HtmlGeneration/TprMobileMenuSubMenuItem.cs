using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprMobileMenuSubMenuItem
    {
        public AttributeDictionary? Attriubutes {  get; set; }
        public string? Id { get; set; }
        public string? LinkText { get; set; }
        public string? LinkDestination { get; set; }
    }
}
