using GovUk.Frontend.AspNetCore.Extensions;
using GovUk.Frontend.Umbraco.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Blocks
{
    /// <summary>
    /// Task list summaries automatically count the tasks they summarise. However, the status of each task is likely to be set in the controller,
    /// and this is the last point where we have access to the overridden properties of the tasks before rendering the task list summary.
    /// So we have to query the tasks here and pass the result down to the task list summary, which we can do using ModelState.
    /// </summary>
    public class TaskListSummaryActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var model = (context.Result as ViewResult)?.Model;
            if (model is null) { return; }

            var publishedContentModel = model as PublishedContentModel;
            if (publishedContentModel is null)
            {
                foreach (var property in model!.GetType().GetPublicProperties())
                {
                    if (property.PropertyType.IsAssignableTo(typeof(PublishedContentModel)))
                    {
                        publishedContentModel = (PublishedContentModel?)property.GetValue(model);
                        break;
                    }
                }
            }
            if (publishedContentModel is not null)
            {
                var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
                Func<OverridableBlockListItem, bool> blockListFilter = x => true;
                Func<OverridableBlockGridItem, bool> blockGridFilter = x => true;
                foreach (var property in publishedContentModel!.GetType().GetPublicProperties())
                {
                    if (property.PropertyType.IsAssignableTo(typeof(OverridableBlockListModel)))
                    {
                        var blockList = (OverridableBlockListModel?)property.GetValue(publishedContentModel);
                        if (blockList is not null)
                        {
                            blockListFilter = blockList.Filter;
                            blocks.AddRange(blockList);
                        }
                    }
                    else if (property.PropertyType.IsAssignableTo(typeof(OverridableBlockGridModel)))
                    {
                        var blockGrid = (OverridableBlockGridModel?)property.GetValue(publishedContentModel);
                        if (blockGrid is not null)
                        {
                            blockGridFilter = blockGrid.Filter;
                            blocks.AddRange(blockGrid);
                        }
                    }
                }

                var taskListSummaries = blocks.FindBlocksByContentTypeAlias(ElementTypeAliases.TaskListSummary);
                if (taskListSummaries.Any())
                {
                    Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> combinedFilter = block =>
                    {
                        if (block is OverridableBlockListItem listItem) { return blockListFilter(listItem); }
                        if (block is OverridableBlockGridItem gridItem) { return blockGridFilter(gridItem); }
                        return true;
                    };
                    Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>?> blockSelector = block =>
                    {
                        if (block is OverridableBlockListItem listItem) { return listItem; }
                        if (block is OverridableBlockGridItem gridItem) { return gridItem; }
                        return null;
                    };
                    var tasks = blocks.FindBlocksByContentTypeAlias(ElementTypeAliases.Task)
                        .Where(combinedFilter).Select(blockSelector).OfType<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
                    var taskStatuses = tasks
                        .Select(x => x.Settings.Value<string>(PropertyAliases.TaskListTaskStatus))
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Select(x => Enum.Parse<TaskListTaskStatus>(x!.Replace(" ", string.Empty), true));
                    foreach (var taskListSummary in taskListSummaries)
                    {
                        context.ModelState.SetInitialValue(taskListSummary.Content.Key.ToString(), string.Join(",", taskStatuses));
                    }
                }
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
