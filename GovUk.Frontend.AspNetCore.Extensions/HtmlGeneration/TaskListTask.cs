using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TaskListTask
    {
        public AttributeDictionary? Attributes { get; set; }
        public TaskName Name { get; set; } = new();
        public Link? Link { get; set; }
        public TaskStatus Status { get; set; } = new();
        public Hint? Hint { get; set; }
    }
}