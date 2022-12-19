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
        /// <summary>
        /// Recursively find the first matching block in any block list on the given content node
        /// </summary>
        /// <param name="content">The content node to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
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
        /// Recursively find the matching blocks in all block lists on the given content node
        /// </summary>
        /// <param name="content">The content node to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IPublishedContent content, Func<BlockListItem, bool> matcher)
        {
            var result = new List<BlockListItem>();
            var blockLists = content.GetBlockListModels();

            foreach (var blockList in blockLists)
            {
                if (blockList != null)
                {
                    var blocks = blockList.FindBlocks(matcher);
                    if (blocks.Any())
                    {
                        result.AddRange(blocks);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Recursively find the first block in any block list on the given content node that is bound to a model property using the 'Model property' Umbraco property in the block's settings
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
