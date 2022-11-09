using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace GovUk.Frontend.AspNetCore.Extensions.HtmlGeneration
{
    public class TaskListRow
    {
        public TaskListRowKey? Key { get; set; }
        public TaskListRowValue? Value { get; set; }
        public AttributeDictionary? Attributes { get; set; }
        public TaskListRowActions? Actions { get; set; }
    }

    public class TaskListRowActions
    {
        public IReadOnlyList<TaskListRowAction>? Items { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class TaskListRowAction
    {
        public string? VisuallyHiddenText { get; set; }
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class TaskListRowKey
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }

    public class TaskListRowValue
    {
        public IHtmlContent? Content { get; set; }
        public AttributeDictionary? Attributes { get; set; }
    }
}