using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend.TagHelpers
{
    /// <summary>
    /// Represents the status of a task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListStatusElement)]
    public class TaskListTaskStatusTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task-status";
        internal const string StatusTagElement = "strong";
        internal const string TagAttributesPrefix = "tag-";

        /// <summary>
        /// The status of the task. Set to <c>null</c> if no status is possible or the status does not fit one of the standard values.
        /// </summary>
        [HtmlAttributeName("status")]
        public TaskListTaskStatus? Status { get; set; }

        /// <summary>
        /// Sets whether to display the status as a <c>.govuk-tag</c>.
        /// </summary>
        [HtmlAttributeName("tag")]
        public bool Tag { get; set; } = true;

        /// <summary>
        /// Additional attributes to add to the generated element where <c>.govuk-tag</c> is applied.
        /// </summary>
        [HtmlAttributeName(DictionaryAttributePrefix = TagAttributesPrefix)]

        public IDictionary<string, string?>? TagAttributes { get; set; } = new Dictionary<string, string?>();

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskContext = context.GetContextItem<TaskListTaskContext>();

            IHtmlContent? textContent = null;
            using (context.SetScopedContextItem(taskContext))
            {
                textContent = new HtmlString((await output.GetChildContentAsync()).GetContent());
            }

            taskContext.Status.Attributes = output.Attributes.ToAttributeDictionary();
            taskContext.Status.Status = Status;
            taskContext.Status.Content = textContent;
            if (Tag) { taskContext.Status.Tag = new Tag { Attributes = TagAttributes.ToAttributeDictionary() }; }

            output.SuppressOutput();
        }
    }
}