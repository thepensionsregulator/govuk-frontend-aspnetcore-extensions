using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.HtmlGeneration
{
    public record TprFooterThreeColumnLinks
    {
        public AttributeDictionary? Attributes { get; set; }
       public IList<TprFooterThreeColumnLink>? ThreeColumnFooterLinks { get; set; }
    }
}
