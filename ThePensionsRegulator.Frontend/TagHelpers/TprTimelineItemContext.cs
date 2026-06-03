using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using GovUk.Frontend.AspNetCore.Extensions.TagHelpers;
using GovUk.Frontend.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprTimelineItemContext
    {
        public string? DateTime { get; set; }
        public string? Heading { get; set; }

        public IHtmlContent HtmlContent { get; set; } = new HtmlString(string.Empty);
    }
}
