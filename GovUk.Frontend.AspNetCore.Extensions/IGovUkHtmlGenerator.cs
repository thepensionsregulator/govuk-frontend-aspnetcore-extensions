using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public interface IGovUkHtmlGenerator
    {
        TagBuilder GenerateTprBackToTop(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprBackToMenu(string href, IHtmlContent content, AttributeDictionary? attributes);
        TagBuilder GenerateTprHeaderBar(TprHeaderBar tprHeaderBar);
        TagBuilder GenerateTprFooterBar(TprFooterBar tprFooterBar);
        TagBuilder GenerateTprContextBar(TprContextBar tprContextBar);
        TagBuilder GenerateTaskList(AttributeDictionary? attributes, IEnumerable<TaskListSection> sections);
        TagBuilder GenerateTaskListSummary(TaskListSummary taskListSummary);
        TagBuilder GenerateSummaryCard(SummaryCard summaryCard);
    }
}
