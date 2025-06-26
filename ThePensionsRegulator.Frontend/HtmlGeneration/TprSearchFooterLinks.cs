using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprSearchFooterLinks
    {
        public required IList<(AttributeDictionary Attributes, IHtmlContent Content)> Links { get; set; }
    }
}
