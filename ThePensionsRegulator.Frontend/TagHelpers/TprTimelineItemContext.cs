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
