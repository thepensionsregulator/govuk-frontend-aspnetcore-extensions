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
        
        public AttributeDictionary? Attributes { get; set; }    
        private readonly List<TprFooterBarThreeColumnLinkContext> _threeColumnLinks = new();
        public IReadOnlyList<TprFooterBarThreeColumnLinkContext> ThreeColumnLinks => _threeColumnLinks;

        public void AddLink(TprFooterBarThreeColumnLinkContext link)
        {
            _threeColumnLinks.Add(link);
        }
        public void SetThreeColumnLinks(AttributeDictionary attributes, List<TprFooterBarThreeColumnLinkContext> links)
        {
            Attributes = attributes;
            _threeColumnLinks.Clear();
            _threeColumnLinks.AddRange(links);
        }
    }
}
