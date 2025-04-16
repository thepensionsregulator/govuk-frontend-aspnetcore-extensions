using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprRelatedLinks
    {
        public required AttributeDictionary RelatedLinksAttributes { get; set; }
        public required AttributeDictionary HeadingAttributes { get; set; }
        public required IHtmlContent HeadingContent { get; set; }
        public required IList<(AttributeDictionary Attributes, IHtmlContent Content)> Links { get; set; }
    }
}
