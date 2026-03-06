using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ThePensionsRegulator.GovUk.Frontend.HtmlGeneration;

namespace ThePensionsRegulator.GovUk.Frontend.TagHelpers
{
    /// <summary>
    /// Represents a hint for a task in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListHintElement)]
    public class TaskListTaskHintTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-task-hint";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskContext = context.GetContextItem<TaskListTaskContext>();

            HtmlString? htmlContent = null;
            using (context.SetScopedContextItem(taskContext))
            {
                var content = (await output.GetChildContentAsync()).GetContent();
                if (!string.IsNullOrEmpty(content))
                {
                    htmlContent = new HtmlString(content);
                }
            }

            taskContext.Hint = new Hint { Attributes = output.Attributes.ToAttributeDictionary(), Content = htmlContent };

            output.SuppressOutput();
        }
    }
}