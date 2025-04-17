using AngleSharp.Diffing.Extensions;
using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TaskStatus = GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration.TaskStatus;

namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    public partial class ComponentTests
    {
        [TestCaseSource(typeof(ComponentFixtureData), nameof(ComponentFixtureData.GetTaskListData))]
        public void TaskList(ComponentTestCaseData<OptionsJson.TaskList> data) =>
            CheckComponentHtmlMatchesExpectedHtml(
                data,
                (generator, options) =>
                {
                    var tasks = new List<TaskListTask?>();
                    foreach (var item in options.Items)
                    {
                        if (item is null)
                        {
                            tasks.Add(null);
                        }
                        else
                        {
                            AttributeDictionary taskAttributes = [];
                            if (!string.IsNullOrEmpty(item.Classes)) { taskAttributes.Add("class", item.Classes); }

                            var taskName = new TaskName { Content = TextOrHtmlHelper.GetHtmlContent(item.Title.Text, item.Title.Html)! };
                            if (!string.IsNullOrEmpty(item.Title.Classes) && string.IsNullOrEmpty(item.Href)) { taskName.Attributes.Add("class", item.Title.Classes); }

                            Link? linkToTask = !string.IsNullOrEmpty(item.Href) ? new Link { Href = item.Href } : null;
                            if (!string.IsNullOrEmpty(item.Title.Classes) && linkToTask is not null) { linkToTask.Attributes.Add("class", item.Title.Classes); }

                            var hint = item.Hint is not null ? new Hint { Content = TextOrHtmlHelper.GetHtmlContent(item.Hint.Text, item.Hint.Html) } : null;

                            var status = BuildStatus(item);

                            tasks.Add(new TaskListTask
                            {
                                Attributes = taskAttributes,
                                Name = taskName,
                                Link = linkToTask,
                                Hint = hint,
                                Status = status
                            });
                        }
                    }

                    var taskListAttributes = options.Attributes.ToAttributeDictionary();
                    if (!string.IsNullOrEmpty(options.Classes)) { taskListAttributes.MergeAttribute("class", options.Classes); }
                    if (!string.IsNullOrEmpty(options.IdPrefix)) { taskListAttributes.Add("id", options.IdPrefix); }

                    return generator.GenerateTaskList(taskListAttributes, tasks).ToHtmlString();
                });

        private static TaskStatus BuildStatus(OptionsJson.TaskListTask item)
        {
            var status = new TaskStatus();

            if (!string.IsNullOrEmpty(item.Status.Classes))
            {
                status.Attributes.MergeCssClass(item.Status.Classes);
            }

            if (item.Status.Tag is not null)
            {
                status.Tag = new();
                status.Tag.Attributes.AddRange(item.Status.Tag.Attributes);

                if (!string.IsNullOrEmpty(item.Status.Tag.Classes))
                {
                    status.Tag.Attributes.MergeCssClass(item.Status.Tag.Classes);
                }
                status.Content = TextOrHtmlHelper.GetHtmlContent(item.Status.Tag.Text, item.Status.Tag.Html);
            }

            if (!string.IsNullOrEmpty(item.Status.Text) || !string.IsNullOrEmpty(item.Status.Html))
            {
                status.Content = TextOrHtmlHelper.GetHtmlContent(item.Status.Text, item.Status.Html);
            }

            if (Enum.TryParse(typeof(TaskListTaskStatus), status.Content?.ToHtmlString(), out var parsedStatus))
            {
                status.Status = (TaskListTaskStatus)parsedStatus;
            }

            return status;
        }
    }
}
