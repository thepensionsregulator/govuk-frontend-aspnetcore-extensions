using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace ThePensionsRegulator.Frontend.TagHelpers
{
    internal class TprSearchFooterLinksContext
    {
        public IList<(AttributeDictionary Attributes, IHtmlContent Content)> Links = new List<(AttributeDictionary Attributes, IHtmlContent Content)>();

        public void AddLink(AttributeDictionary attributes, IHtmlContent content)
        {
            Links.Add((attributes, content));
        }
    }
}
