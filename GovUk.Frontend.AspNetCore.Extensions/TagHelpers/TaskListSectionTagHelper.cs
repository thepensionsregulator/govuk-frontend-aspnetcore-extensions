using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    [HtmlTargetElement(TagName)]
    [RestrictChildren(TaskListSectionNameTagHelper.TagName, TaskListTaskTagHelper.TagName)]
    [OutputElementHint(ComponentGenerator.TaskListElement)]
    public class TaskListSectionTagHelper :  TagHelper
    {
        internal const string TagName = "govuk-task-list-section";

        private readonly IGovUkHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Creates a new <see cref="TaskListSectionTagHelper"/>.
        /// </summary>
        public TaskListSectionTagHelper()
            : this(htmlGenerator: null)
        {
        }

        internal TaskListSectionTagHelper(IGovUkHtmlGenerator? htmlGenerator)
        {
            _htmlGenerator = htmlGenerator ?? new ComponentGenerator();
        }

        /// <inheritdoc/>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            var taskListContext = context.GetContextItem<TaskListContext>();

            var taskListSectionContext = new TaskListSectionContext();

            using (context.SetScopedContextItem(taskListSectionContext))
            {
                await output.GetChildContentAsync();
            }

            taskListContext.AddSection(new TaskListSection
            {
                Name = taskListSectionContext.Name,
                Tasks = taskListSectionContext.Tasks
            });

            output.SuppressOutput();
        }
    }
}
