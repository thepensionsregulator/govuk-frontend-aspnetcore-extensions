using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    /// <summary>
    /// Represents the section heading in a GOV.UK task list component.
    /// </summary>
    [HtmlTargetElement(TagName, ParentTag = TaskListSectionTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListSectionNameElement)]
    public class TaskListSectionNameTagHelper : TagHelper
    {
        internal const string TagName = "govuk-task-list-section-name";

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var taskSectionContext = context.GetContextItem<TaskListSectionContext>();

            HtmlString? htmlContent = null;
            using (context.SetScopedContextItem(taskSectionContext))
            {
                var content = (await output.GetChildContentAsync()).GetContent();
                if (!string.IsNullOrEmpty(content))
                {
                    htmlContent = new HtmlString(content);
                }
            }

            taskSectionContext.Name = (output.Attributes.ToAttributeDictionary(), htmlContent);

            output.SuppressOutput();
        }
    }
}
