using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprSearchBarContext
    {
        public AttributeDictionary? SearchAttributes { get; set; }
        public IHtmlContent? SearchBoxPrompt { get; set; }
        public bool SearchBoxAllowHtml { get; set; }
    }
}
