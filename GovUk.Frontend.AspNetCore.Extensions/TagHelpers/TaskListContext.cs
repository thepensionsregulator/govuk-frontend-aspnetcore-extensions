using GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration;
using GovUk.Frontend.AspNetCore.HtmlGeneration;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.TagHelpers
{
    internal class TaskListContext
    {
        private readonly List<TaskListRow> _rows;

        public TaskListContext()
        {
            _rows = new List<TaskListRow>();
        }

        public IReadOnlyList<TaskListRow> Rows => _rows;

        public void AddRow(TaskListRow row)
        {
            Guard.ArgumentNotNull(nameof(row), row);

            _rows.Add(row);
        }
    }
}