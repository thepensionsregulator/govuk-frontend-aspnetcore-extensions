using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprTimelineItemLink
    {
        public AttributeDictionary? Attributes { get; set; }

        public string? Text { get; set; }
        public IHtmlContent Href { get; set; } = new StringHtmlContent("");
    }
}
