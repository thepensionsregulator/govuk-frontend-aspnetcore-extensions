using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend.TagHelpers
{
    /// <summary>
    /// Represents an task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTagHelper.TagName)]
    [RestrictChildren(TaskListTaskNameTagHelper.TagName, TaskListTaskHintTagHelper.TagName, TaskListTaskStatusTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListTaskElement)]
    public class TaskListTaskTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task";
        private const string TaskHrefAttributeName = "href";
        internal const string LinkAttributesPrefix = "link-";

        /// <summary>
        /// Additional attributes to add to the generated <c>a</c> element where <c>.govuk-link</c> is applied.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = LinkAttributesPrefix)]

        public IDictionary<string, string?>? LinkAttributes { get; set; } = new Dictionary<string, string?>();

        /// <summary>
        /// A link to the task.
        /// </summary>
        [HtmlAttributeName(TaskHrefAttributeName)]
        public string? Href { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskListContext = context.GetContextItem<TaskListContext>();

            var taskContext = new TaskListTaskContext();

            using (context.SetScopedContextItem(taskContext))
            {
                await output.GetChildContentAsync();
            }

            taskContext.ThrowIfIncomplete();

            taskListContext.AddTask(new TaskListTask
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Name = taskContext.Name,
                Link = Href is not null ? new Link { Attributes = LinkAttributes.ToAttributeDictionary(), Href = Href } : null,
                Status = taskContext.Status,
                Hint = taskContext.Hint
            });

            output.SuppressOutput();
        }
    }
}