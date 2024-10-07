using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class Link
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public required string Href { get; set; }
    }
}
