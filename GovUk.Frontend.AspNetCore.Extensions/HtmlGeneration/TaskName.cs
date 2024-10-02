using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TaskName
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public IHtmlContent Content { get; set; } = new HtmlString(string.Empty);
    }
}
