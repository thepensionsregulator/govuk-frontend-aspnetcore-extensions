using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateTaskList(AttributeDictionary? attributes, IEnumerable<TaskListTask> tasks);
        TagBuilder GenerateTaskListSummary(TaskListSummary taskListSummary);
        TagBuilder GenerateSummaryCard(SummaryCard summaryCard);
    }
}
