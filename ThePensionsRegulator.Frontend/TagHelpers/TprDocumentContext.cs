using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprDocumentContext
    {
        public string? Href { get; set; }

        public string? KbSize { get; set; }

        public string? Pages { get; set; }

        public IHtmlContent? Document { get; set; }

        public IHtmlContent? DocumentTitle { get; set; }

        public IHtmlContent? DocumentDescription { get; set; }
    }
}
