using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.GovUk.Frontend.HtmlGeneration
{
    public class TaskName
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public IHtmlContent Content { get; set; } = new HtmlString(string.Empty);
    }
}
