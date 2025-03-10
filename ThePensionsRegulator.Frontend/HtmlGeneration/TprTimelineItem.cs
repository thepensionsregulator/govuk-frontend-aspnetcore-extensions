using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprTimelineItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public string? DateTime { get; set; }
        public string? Heading { get; set; }
        public string? ByLine { get; set; }
        public IHtmlContent? Content { get; set; }
    }
}
