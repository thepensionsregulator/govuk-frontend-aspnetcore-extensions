using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
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

            HtmlString? htmlContent = null;
            using (context.SetScopedContextItem(taskContext))
            {
                var content = (await output.GetChildContentAsync()).GetContent();
                if (!string.IsNullOrEmpty(content))
                {
                    htmlContent = new HtmlString(content);
                }
            }

            taskContext.Name = (output.Attributes.ToAttributeDictionary(), htmlContent);

            output.SuppressOutput();
        }
    }
}