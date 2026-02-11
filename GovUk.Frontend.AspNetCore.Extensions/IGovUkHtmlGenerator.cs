using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateTaskList(AttributeDictionary? attributes, IEnumerable<TaskListTask> tasks, string? idPrefix);
        TagBuilder GenerateTaskListSummary(TaskListSummary taskListSummary);
    }
}
