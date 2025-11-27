using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprFooterBarThreeColumnLinkContext
    {
        private (AttributeDictionary Attributes, string LinkText, string LinkUrl, string LanguageCode) _threeColumLink;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _threeColumLink.LinkText;
        public string? LinkUrl => _threeColumLink.LinkUrl;
        public string? LanguageCode => _threeColumLink.LanguageCode;

        public void SetThreeColumnLink(AttributeDictionary attributes, string linkText, string linkUrl, string languageCode)
        {
            _threeColumLink = (attributes, linkText, linkUrl, languageCode);
        }
    }
}
