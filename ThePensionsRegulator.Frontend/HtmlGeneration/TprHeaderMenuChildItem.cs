using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public class TprHeaderMenuChildItem
    {
        public AttributeDictionary? Attributes { get; set; }
        public Guid ContentKey { get; set; }
        public required string LinkText { get; set; }
        public required string LinkUrl { get; set; }

        public TprHeaderMenuChildItem(){}

        [SetsRequiredMembers]
        public TprHeaderMenuChildItem(Guid contentKey, string linkText, string linkUrl)
        {
            ContentKey = contentKey;
            LinkText = linkText;
            LinkUrl = linkUrl;
        }
    }
}
