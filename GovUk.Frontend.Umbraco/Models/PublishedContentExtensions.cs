using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    public static class PublishedContentExtensions
    {
        public static BlockListItem? FindBlock(this IPublishedContent content, Func<BlockListItem, bool> matcher)
        {
            var blockLists = content.GetBlockListModels();

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var block = blockList.FindBlock(matcher);
                    if (block != null)
                    {
                        return block;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Recursively find the first block in a request that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="content"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static BlockListItem? FindBlockByBoundProperty(this IPublishedContent content, string propertyName)
        {
            var blockLists = content.GetBlockListModels();

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var block = blockList.FindBlockByBoundProperty(propertyName);
                    if (block != null)
                    {
                        return block;
                    }
                }
            }
            return null;
        }

        private static IEnumerable<BlockListModel?> GetBlockListModels(this IPublishedContent content)
        {
            if (content?.Properties is null)
            {
                throw new ArgumentNullException(nameof(content.Properties));
            }

            return content.Properties
                .Where(x => x.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && x.HasValue())
                .Select(x => x.Value<BlockListModel>(null));
        }
    }
}
