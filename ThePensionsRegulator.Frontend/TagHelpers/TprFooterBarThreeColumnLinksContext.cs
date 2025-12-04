using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprFooterBarThreeColumnLinksContext
    {
        private (AttributeDictionary Attributes, List<TprFooterBarThreeColumnLinkContext>) _column;
        public AttributeDictionary? Attributes { get; set; }    
        private readonly List<TprFooterBarThreeColumnLinkContext> _threeColumnLinks = new();
        public IReadOnlyList<TprFooterBarThreeColumnLinkContext> ThreeColumnLinks => _threeColumnLinks;

        public void AddLink(TprFooterBarThreeColumnLinkContext link)
        {
            _threeColumnLinks.Add(link);
        }
        public void SetThreeColumnLinks(AttributeDictionary attributes, List<TprFooterBarThreeColumnLinkContext> links)
        {
            _column = (attributes, links);
        }
    }
}
