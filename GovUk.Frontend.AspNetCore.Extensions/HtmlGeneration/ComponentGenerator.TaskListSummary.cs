#nullable enable

using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TaskListSummaryElement = "div";
        internal const int TaskListSummaryDefaultHeadingLevel = 2;
        internal const int TaskListSummaryMinHeadingLevel = 1;
        internal const int TaskListSummaryMaxHeadingLevel = 6;

        public TagBuilder GenerateTaskListSummary(TaskListSummary taskListSummary)
        {
            if (taskListSummary.HeadingLevel < TaskListSummaryMinHeadingLevel || taskListSummary.HeadingLevel > TaskListSummaryMaxHeadingLevel)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(taskListSummary.HeadingLevel)} must be between {TaskListSummaryMinHeadingLevel} and {TaskListSummaryMaxHeadingLevel}.",
                    nameof(taskListSummary.HeadingLevel));
            }

            Guard.ArgumentNotNullOrEmpty(nameof(taskListSummary.IncompleteStatus), taskListSummary.IncompleteStatus);
            Guard.ArgumentNotNullOrEmpty(nameof(taskListSummary.CompletedStatus), taskListSummary.CompletedStatus);
            Guard.ArgumentNotNullOrEmpty(nameof(taskListSummary.Tracker), taskListSummary.Tracker);

            var tagBuilder = new TagBuilder(TaskListSummaryElement);
            if (taskListSummary.Attributes != null) { tagBuilder.MergeAttributes(taskListSummary.Attributes); }
            tagBuilder.MergeCssClass("govuk-task-list-summary");

            var statusTagBuilder = new TagBuilder($"h{taskListSummary.HeadingLevel}");
            statusTagBuilder.MergeCssClass("govuk-heading-s");
            statusTagBuilder.MergeCssClass("govuk-task-list-summary__heading");
            statusTagBuilder.InnerHtml.Append(taskListSummary.CompletedTasks == taskListSummary.TotalTasks ? taskListSummary.CompletedStatus : taskListSummary.IncompleteStatus);
            tagBuilder.InnerHtml.AppendHtml(statusTagBuilder);

            var trackerTagBuilder = new TagBuilder("p");
            trackerTagBuilder.MergeCssClass("govuk-body");
            statusTagBuilder.MergeCssClass("govuk-task-list-summary__tracker");
            trackerTagBuilder.InnerHtml.Append(string.Format(taskListSummary.Tracker, taskListSummary.CompletedTasks, taskListSummary.TotalTasks));
            tagBuilder.InnerHtml.AppendHtml(trackerTagBuilder);

            return tagBuilder;
        }
    }
}