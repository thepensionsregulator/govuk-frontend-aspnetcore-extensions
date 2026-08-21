using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend.TagHelpers
{
    /// <summary>
    /// Represents the name of a task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListTaskNameElement)]
    public class TaskListTaskNameTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task-name";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskContext = context.GetContextItem<TaskListTaskContext>();

            taskContext.Name.Attributes = output.Attributes.ToAttributeDictionary();

            using (context.SetScopedContextItem(taskContext))
            {
                var content = (await output.GetChildContentAsync()).GetContent();
                if (!string.IsNullOrEmpty(content))
                {
                    taskContext.Name.Content = new HtmlString(content);
                }
            }

            output.SuppressOutput();
        }
    }
}