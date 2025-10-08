using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Diagnostics.CodeAnalysis;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuChildItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public required string LinkText { get; set; }
        public required string LinkUrl { get; set; }

        public TprHeaderMenuChildItem() { }

        [SetsRequiredMembers]
        public TprHeaderMenuChildItem(string linkText, string linkUrl)
        {
            LinkText = linkText;
            LinkUrl = linkUrl;
        }
    }
}
