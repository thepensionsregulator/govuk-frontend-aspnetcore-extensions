using GovUk.Frontend.AspNetCore.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents a row in a GDS task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListTagHelper.TagName)]
    [RestrictChildren(TaskListRowKeyTagHelper.TagName, TaskListRowValueTagHelper.TagName, TaskListRowActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListRowElement)]
    public class TaskListRowTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-row";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var TaskListContext = context.GetContextItem<TaskListContext>();

            var rowContext = new TaskListRowContext();

            using (context.SetScopedContextItem(rowContext))
            {
                await output.GetChildContentAsync();
            }

            rowContext.ThrowIfIncomplete();

            TaskListContext.AddRow(new TaskListRow()
            {
                Actions = new TaskListRowActions()
                {
                    Items = rowContext.Actions,
                    Attributes = rowContext.ActionsAttributes
                },
                Attributes = output.Attributes.ToAttributeDictionary(),
                Key = new TaskListRowKey()
                {
                    Content = rowContext.Key!.Value.Content,
                    Attributes = rowContext.Key!.Value.Attributes
                },
                Value = new TaskListRowValue()
                {
                    Content = rowContext.Value!.Value.Content,
                    Attributes = rowContext.Value!.Value.Attributes
                }
            });

            output.SuppressOutput();
        }
    }
}