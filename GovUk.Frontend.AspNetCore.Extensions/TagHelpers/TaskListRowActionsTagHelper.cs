using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the actions wrapper in a GDS task list component row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListRowTagHelper.TagName)]
    [RestrictChildren(TaskListRowActionTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListRowActionsElement)]
    public class TaskListRowActionsTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-row-actions";

        /// <summary>
        /// Creates a new <see cref="TaskListRowActionsTagHelper"/>.
        /// </summary>
        public TaskListRowActionsTagHelper()
        {
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var rowContext = context.GetContextItem<TaskListRowContext>();
            rowContext.SetActionsAttributes(output.Attributes.ToAttributeDictionary());

            await output.GetChildContentAsync();

            output.SuppressOutput();
        }
    }
}