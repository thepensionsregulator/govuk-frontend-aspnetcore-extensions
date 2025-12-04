using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprFooterBar
    {
        public AttributeDictionary? FooterBarAttributes { get; set; }
        public string? LanguageCode { get; set; }
        public AttributeDictionary? LogoAttributes { get; set; }
        public string? LogoHref { get; set; }
        public string? LogoAlternativeText { get; set; }
        public IList<TprFooterThreeColumnLinks>? ThreeColumnLinks { get; set; } 
        public AttributeDictionary? CopyrightAttributes { get; set; }
        public IHtmlContent? Copyright { get; set; }
        public bool CopyrightAllowHtml { get; set; }
        public AttributeDictionary? ContentAttributes { get; set; }
        public IHtmlContent? Content { get; set; }
        public bool ContentAllowHtml { get; set; }
    }
}
