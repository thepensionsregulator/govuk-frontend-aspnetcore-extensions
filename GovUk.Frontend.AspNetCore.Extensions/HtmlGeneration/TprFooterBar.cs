using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TprFooterBar
    {
        public AttributeDictionary? FooterBarAttributes { get; set; }
        public AttributeDictionary? LogoAttributes { get; set; }
        public string? LogoHref { get; set; }
        public string? LogoAlternativeText { get; set; }
        public AttributeDictionary? CopyrightAttributes { get; set; }
        public IHtmlContent? Copyright { get; set; }
        public bool CopyrightAllowHtml { get; set; }
        public AttributeDictionary? ContentAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public bool ContentAllowHtml { get; set; }
    }
}
