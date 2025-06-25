using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprHeaderBar
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
        public bool DisplaySearch {  get; set; }
        public AttributeDictionary? SearchAttributes { get; set; }
        public string? ActionPath {  get; set; }     
        public string? AutoCompleteUrl {  get; set; }     
        public string? SearchPlaceholderText {  get; set; }
        public string? SearchAriaLabel {  get; set; }
        public string? SearchInputQuery { get; set; } 
        public bool DisplayMobileMenu {  get; set; }
        public AttributeDictionary? MobileMenuAttributes { get; set ; }
        public List<TprMobileMenuParentItem>? MobileMenuItems { get; set; } = new();

    }
}
