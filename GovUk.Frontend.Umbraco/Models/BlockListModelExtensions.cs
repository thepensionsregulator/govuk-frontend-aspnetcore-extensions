using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Models
{
    public static class BlockListModelExtensions
    {
        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return RecursivelyFindBlock(blockList, matcher);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return RecursivelyFindBlocks(blockList, matcher, false);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListItem> blockList, string propertyName)
        {
            return RecursivelyFindBlock(blockList, x => x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListItem> blockList, string alias)
        {
            return RecursivelyFindBlock(blockList, x => x.Content.ContentType.Alias == alias);
        }

        private static BlockListItem? RecursivelyFindBlock(IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return RecursivelyFindBlocks(blockList, matcher, true).FirstOrDefault();
        }

        private static IList<BlockListItem> RecursivelyFindBlocks(IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher, bool returnFirstMatchOnly)
        {
            if (blockList is null)
            {
                throw new ArgumentNullException(nameof(blockList));
            }

            var matchedBlocks = new List<BlockListItem>();

            foreach (var block in blockList)
            {
                if (matcher(block))
                {
                    matchedBlocks.Add(block);
                    if (returnFirstMatchOnly) { return matchedBlocks; }
                }

                foreach (var blockProperty in block.Content.Properties)
                {
                    if (blockProperty.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && blockProperty.HasValue())
                    {
                        IEnumerable<BlockListItem>? childBlocks = (block as OverridableBlockListItem)?.Content.Value<OverridableBlockListModel>(blockProperty.Alias);
                        if (childBlocks == null) { childBlocks = blockProperty.Value<BlockListModel>(null); }
                        var result = RecursivelyFindBlocks(childBlocks!, matcher, returnFirstMatchOnly);
                        if (result.Any())
                        {
                            matchedBlocks.AddRange(result);
                        }
                    }
                }
            }
            return matchedBlocks;
        }
    }
}
