using GovUk.Frontend.AspNetCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public class TaskListTaskStatusProvider : ITaskListTaskStatusProvider
    {
        /// <inheritdoc/>
        public IEnumerable<TaskListTaskStatus> FindTaskStatuses(IPublishedContent content)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            Func<OverridableBlockListItem, bool> blockListFilter = x => true;
            Func<OverridableBlockGridItem, bool> blockGridFilter = x => true;
            foreach (var blockModel in content.FindOverridableBlockModels())
            {
                if (blockModel is OverridableBlockListModel blockList)
                {
                    blockListFilter = blockList.Filter;
                    blocks.AddRange(blockList);
                }
                else if (blockModel is OverridableBlockGridModel blockGrid)
                {
                    blockGridFilter = blockGrid.Filter;
                    blocks.AddRange(blockGrid);
                }
            }

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

            return taskStatuses;
        }
    }
}
