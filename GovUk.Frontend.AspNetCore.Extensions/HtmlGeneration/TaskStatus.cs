using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TaskStatus
    {
        public AttributeDictionary Attributes { get; set; } = [];
        public Tag? Tag { get; set; }
        public TaskListTaskStatus? Status { get; set; }
        public IHtmlContent? Content { get; set; }
    }
}
