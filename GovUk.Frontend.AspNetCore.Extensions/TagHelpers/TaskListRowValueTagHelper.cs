using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the value in a GDS task list component row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListRowTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListRowValueElement)]
    public class TaskListRowValueTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-row-value";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var TaskListRowContext = context.GetContextItem<TaskListRowContext>();

            var content = await output.GetChildContentAsync();

            TaskListRowContext.SetValue(output.Attributes.ToAttributeDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}
