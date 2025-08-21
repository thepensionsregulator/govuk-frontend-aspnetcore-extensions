using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuChildItem
    {
        public AttributeDictionary? Attriubutes { get; set; }
        public string? LinkText { get; set; }
        public string? LinkDestination { get; set; }

        public TprHeaderMenuChildItem(){}

        public TprHeaderMenuChildItem(AttributeDictionary? attriubutes, string? linkText, string? linkDestination)
        {
            Attriubutes = attriubutes;
            LinkText = linkText;
            LinkDestination = linkDestination;
        }
    }
}
