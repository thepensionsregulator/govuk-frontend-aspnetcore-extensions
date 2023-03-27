using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TprContextBar
    {
        public AttributeDictionary? ContextBarAttributes { get; set; }
        public AttributeDictionary? Context1Attributes { get; set; }
        public IHtmlContent? Context1Content { get; set; }
        public bool Context1AllowHtml { get; set; }
        public AttributeDictionary? Context2Attributes { get; set; }
        public IHtmlContent? Context2Content { get; set; }
        public bool Context2AllowHtml { get; set; }
        public AttributeDictionary? Context3Attributes { get; set; }
        public IHtmlContent? Context3Content { get; set; }
        public bool Context3AllowHtml { get; set; }
    }
}
