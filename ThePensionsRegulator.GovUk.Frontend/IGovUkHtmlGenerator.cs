using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateTaskList(AttributeDictionary? attributes, IEnumerable<TaskListTask> tasks, string? idPrefix);
        TagBuilder GenerateTaskListSummary(TaskListSummary taskListSummary);
    }
}
