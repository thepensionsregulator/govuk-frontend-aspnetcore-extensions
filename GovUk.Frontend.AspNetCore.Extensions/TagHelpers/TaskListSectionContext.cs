using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TaskListSectionContext
    {
        public (AttributeDictionary Attributes, HtmlString? Content) Name { get; internal set; }

        private readonly List<TaskListTask> _tasks;

        public TaskListSectionContext()
        {
            _tasks = new List<TaskListTask>();
        }

        public IReadOnlyList<TaskListTask> Tasks => _tasks;

        public void AddTask(TaskListTask task)
        {
            Guard.ArgumentNotNull(nameof(task), task);

            _tasks.Add(task);
        }

        public void ThrowIfIncomplete()
        {
            if (Tasks.Count < 1)
            {
                throw ExceptionHelper.AChildElementMustBeProvided(TaskListSectionTagHelper.TagName);
            }
        }
    }
}
