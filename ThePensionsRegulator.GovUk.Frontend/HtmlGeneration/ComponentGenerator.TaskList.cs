using GovUk.Frontend.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Encodings.Web;
using ThePensionsRegulator.GovUk.Frontend.TagHelpers;

namespace ThePensionsRegulator.GovUk.Frontend.HtmlGeneration
{
    public partial class ComponentGenerator
    {
        internal const string TaskListElement = "ul";
        internal const string TaskListTaskElement = "li";
        internal const string TaskListTaskNameElement = "div";
        internal const string TaskListHintElement = "div";
        internal const string TaskListStatusElement = "div";

        public TagBuilder GenerateTaskList(
            AttributeDictionary? attributes,
            IEnumerable<TaskListTask?> tasks,
            string? idPrefix)
        {
            Guard.ArgumentNotNull(nameof(tasks), tasks);
            Guard.ArgumentValid(nameof(tasks), "A task list must contain at least one task", tasks.Any());

            var taskListTagBuilder = new TagBuilder(TaskListElement);
            if (attributes is not null) { taskListTagBuilder.MergeAttributes(attributes); }
            taskListTagBuilder.AddCssClass("govuk-task-list");

            if (string.IsNullOrEmpty(idPrefix)) { idPrefix = "task-list"; }

            var taskNumber = 0;
            foreach (var task in tasks)
            {
                taskNumber++;
                if (task is null) { continue; }

                Guard.ArgumentValid(nameof(tasks), "Task name cannot be null or empty", task.Name.Content != null);

                var hintId = BuildHintId(task.Hint, taskNumber, idPrefix);
                var statusId = BuildStatusId(task.Status, taskNumber, idPrefix);

                var taskTagBuilder = new TagBuilder(TaskListTaskElement);
                if (task.Attributes is not null) { taskTagBuilder.MergeAttributes(task.Attributes); }
                taskListTagBuilder.InnerHtml.AppendHtml(taskTagBuilder);

                if (ShouldLinkToTask(task))
                {
                    taskTagBuilder.AddCssClass("govuk-task-list__item--with-link");
                }
                taskTagBuilder.AddCssClass("govuk-task-list__item");

                taskTagBuilder.InnerHtml.AppendHtml(BuildNameAndHint(task, hintId, statusId));

                if (!string.IsNullOrEmpty(statusId))
                {
                    taskTagBuilder.InnerHtml.AppendHtml(BuildStatus(task, statusId));
                }
            }

            return taskListTagBuilder;
        }

        private static TagBuilder BuildNameAndHint(TaskListTask task, string? hintId, string? statusId)
        {
            var taskNameAndHintTagBuilder = new TagBuilder(TaskListTaskNameElement);
            taskNameAndHintTagBuilder.AddCssClass("govuk-task-list__name-and-hint");

            if (ShouldLinkToTask(task))
            {
                taskNameAndHintTagBuilder.InnerHtml.AppendHtml(BuildLinkToTask(task, statusId, hintId));
            }
            else
            {
                taskNameAndHintTagBuilder.InnerHtml.AppendHtml(BuildUnlinkedTaskName(task));
            }

            if (!string.IsNullOrEmpty(hintId))
            {
                taskNameAndHintTagBuilder.InnerHtml.AppendHtml(BuildHint(task, hintId));
            }

            return taskNameAndHintTagBuilder;
        }

        private static bool ShouldLinkToTask(TaskListTask task)
        {
            return !string.IsNullOrEmpty(task.Link?.Href) && task.Status.Status != TaskListTaskStatus.NotApplicable && task.Status.Status != TaskListTaskStatus.CannotStartYet;
        }

        private TagBuilder BuildStatus(TaskListTask task, string? statusId)
        {
            var statusOuterTagBuilder = new TagBuilder(TaskListStatusElement);

            if (task.Status.Attributes is not null)
            {
                statusOuterTagBuilder.MergeAttributes(task.Status.Attributes);
            }
            statusOuterTagBuilder.AddCssClass("govuk-task-list__status");
            if (!statusOuterTagBuilder.Attributes.ContainsKey("id")) { statusOuterTagBuilder.MergeAttribute("id", statusId); }

            var statusText = task.Status.Status.AsText(task.Status.Content);
            if (task.Status.Tag is not null)
            {
                var statusInnerTagBuilder = new TagBuilder(TaskListTaskStatusTagHelper.StatusTagElement);
                statusInnerTagBuilder.MergeAttributes(task.Status.Tag.Attributes);
                statusInnerTagBuilder.AddCssClass("govuk-tag");

                statusInnerTagBuilder.InnerHtml.AppendHtml(statusText);
                statusOuterTagBuilder.InnerHtml.AppendHtml(statusInnerTagBuilder);
            }
            else
            {
                statusOuterTagBuilder.InnerHtml.AppendHtml(statusText);
            }

            return statusOuterTagBuilder;
        }

        private static TagBuilder BuildHint(TaskListTask task, string? hintId)
        {
            var hintTagBuilder = new TagBuilder(TaskListHintElement);
            hintTagBuilder.Attributes.Add("id", hintId);
            hintTagBuilder.MergeAttributes(task.Hint!.Attributes);
            hintTagBuilder.AddCssClass("govuk-task-list__hint");
            hintTagBuilder.InnerHtml.AppendHtml(task.Hint.Content!);
            return hintTagBuilder;
        }

        private static string? BuildStatusId(TaskStatus status, int taskNumber, string idPrefix)
        {
            if (!status.Status.HasValue && string.IsNullOrEmpty(status.Content?.ToHtmlString(HtmlEncoder.Default))) { return null; }

            string statusId = idPrefix;
            if (status.Attributes.ContainsKey("id"))
            {
                statusId = status.Attributes["id"]!;
            }

            statusId = $"{statusId}-{taskNumber}-status";
            return statusId;
        }

        private static string? BuildHintId(Hint? hint, int taskNumber, string idPrefix)
        {
            if (hint?.Content is null) { return null; }

            string hintId = idPrefix;
            if (hint.Attributes.ContainsKey("id"))
            {
                hintId = hint.Attributes["id"]!;
            }

            hintId = $"{hintId}-{taskNumber}-hint";
            return hintId;
        }

        private static TagBuilder BuildUnlinkedTaskName(TaskListTask task)
        {
            var unlinkedTaskTagBuilder = new TagBuilder("div");
            unlinkedTaskTagBuilder.MergeAttributes(task.Name.Attributes);
            unlinkedTaskTagBuilder.InnerHtml.AppendHtml(task.Name.Content!);
            return unlinkedTaskTagBuilder;
        }

        private static TagBuilder BuildLinkToTask(TaskListTask task, string? statusId, string? hintId)
        {
            if (task?.Link is null) { throw new ArgumentException($"{nameof(task)} cannot be null and must have a {nameof(task.Link)}.", nameof(task)); }

            var taskLinkTagBuilder = new TagBuilder("a");
            taskLinkTagBuilder.MergeAttribute("href", task.Link.Href);
            taskLinkTagBuilder.MergeAttributes(task.Link.Attributes);
            taskLinkTagBuilder.AddCssClass("govuk-task-list__link");
            taskLinkTagBuilder.AddCssClass("govuk-link");
            taskLinkTagBuilder.InnerHtml.AppendHtml(task.Name.Content!);

            var linkDescribedBy = new List<string>();
            if (!string.IsNullOrEmpty(hintId)) { linkDescribedBy.Add(hintId); }
            if (!string.IsNullOrEmpty(statusId)) { linkDescribedBy.Add(statusId); }
            if (linkDescribedBy.Any()) { taskLinkTagBuilder.MergeAttribute("aria-describedby", string.Join(' ', linkDescribedBy)); }

            return taskLinkTagBuilder;
        }
    }
}