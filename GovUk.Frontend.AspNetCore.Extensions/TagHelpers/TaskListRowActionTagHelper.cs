using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents an action in a GDS task list row.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListRowActionsTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListRowActionElement)]
    public class TaskListRowActionTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-row-action";

        private const string VisuallyHiddenTextAttributeName = "visually-hidden-text";

        /// <summary>
        /// The visually hidden text for the action link.
        /// </summary>
        [HtmlAttributeName(VisuallyHiddenTextAttributeName)]
        public string? VisuallyHiddenText { get; set; }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var TaskListRowContext = context.GetContextItem<TaskListRowContext>();

            var content = await output.GetChildContentAsync();

            TaskListRowContext.AddAction(new TaskListRowAction()
            {
                Attributes = output.Attributes.ToAttributeDictionary(),
                Content = content.Snapshot(),
                VisuallyHiddenText = VisuallyHiddenText
            });

            output.SuppressOutput();
        }
    }
}