using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderSearch
    {
        public AttributeDictionary? SearchAttributes { get; set; }
        public string? SearchBoxLabel { get; set; }
        public IHtmlContent? SearchBoxPrompt { get; set; }
        public string? LanguagePrefix {  get; set; } //retrive language settings???
        public bool SearchBoxAllowHtml { get; set; }
    }
}
