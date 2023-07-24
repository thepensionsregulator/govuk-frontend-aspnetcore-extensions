using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TaskListSection
    {
        public AttributeDictionary? Attributes { get; set; }
        public (AttributeDictionary Attributes, HtmlString? Content) Name { get; init; }        
        public IEnumerable<TaskListTask> Tasks { get; set; } = new List<TaskListTask>();
    }
}