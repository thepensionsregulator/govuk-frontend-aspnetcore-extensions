using GovUk.Frontend.AspNetCore.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public interface ITaskListTaskStatusProvider
    {
        /// <summary>
        /// Finds the statuses for tasks in zero or more task lists on the given content item.
        /// </summary>
        IEnumerable<TaskListTaskStatus> FindTaskStatuses(IPublishedContent content);
    }
}