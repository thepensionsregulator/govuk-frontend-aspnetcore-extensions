using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using GovUk.Frontend.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the value in a GDS task list component row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListRowTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListRowKeyElement)]
    public class TaskListRowKeyTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-row-key";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var TaskListRowContext = context.GetContextItem<TaskListRowContext>();

            var content = await output.GetChildContentAsync();

            TaskListRowContext.SetKey(output.Attributes.ToAttributeDictionary(), content.Snapshot());

            output.SuppressOutput();
        }
    }
}