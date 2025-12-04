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
        private (AttributeDictionary Attributes, string LinkText, string LinkUrl) _threeColumLink;
        public AttributeDictionary? Attributes { get; set; }
        public string? LinkText => _threeColumLink.LinkText;
        public string? LinkUrl => _threeColumLink.LinkUrl;

        public void SetThreeColumnLink(AttributeDictionary attributes, string linkText, string linkUrl)
        {
            _threeColumLink = (attributes, linkText, linkUrl);
        }
    }
}
