using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class SummaryCardActions
    {
        public IReadOnlyList<SummaryCardAction>? Items { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}
