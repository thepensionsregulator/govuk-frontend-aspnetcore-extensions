using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TprHeaderBar
    {
        public AttributeDictionary? HeaderBarAttributes { get; set; }
        public AttributeDictionary? LogoAttributes { get; set; }
        public string? LogoHref { get; set; }
        public string? LogoAlternativeText { get; set; }
        public AttributeDictionary? LabelAttributes { get; set; }
        public IHtmlContent? Label { get; set; }
        public bool LabelAllowHtml { get; set; }
        public AttributeDictionary? ContentAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public bool ContentAllowHtml { get; set; }
    }
}
