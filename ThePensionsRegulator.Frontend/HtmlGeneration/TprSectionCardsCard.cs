using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprSectionCardsCard
    {
        public AttributeDictionary? CardAttributes { get; set; }
        public AttributeDictionary? TitleAttributes { get; set; }
        public IHtmlContent? Title { get; set; }
        public string? TitleUrl { get; set; }
        public bool TitleAllowHtml { get; set; }
        public AttributeDictionary? ContentAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public bool ContentAllowHtml { get; set; }
    }
}
