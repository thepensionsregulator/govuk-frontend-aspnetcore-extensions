using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ThePensionsRegulator.GovUk.Frontend.HtmlGeneration
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