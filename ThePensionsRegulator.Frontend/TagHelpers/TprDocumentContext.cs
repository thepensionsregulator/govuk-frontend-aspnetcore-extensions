using Microsoft.AspNetCore.Html;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    public class TprDocumentContext
    {
        public string? Href { get; set; }

        public string? KbSize { get; set; }

        public string? Pages { get; set; }

        public string? DatePublished { get; set; }

        public IHtmlContent? Document { get; set; }

        public IHtmlContent? DocumentTitle { get; set; }

        public IHtmlContent? DocumentDescription { get; set; }
    }
}
