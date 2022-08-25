using System;
using System.Collections.Generic;
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
            if (blockList is null)
            {
                throw new ArgumentNullException(nameof(blockList));
            }

            foreach (var block in blockList)
            {
                if (matcher(block))
                {
                    return block;
                }

                foreach (var blockProperty in block.Content.Properties)
                {
                    if (blockProperty.PropertyType.EditorAlias == Constants.PropertyEditors.Aliases.BlockList && blockProperty.HasValue())
                    {
                        IEnumerable<BlockListItem>? childBlocks = (block as OverridableBlockListItem)?.Content.Value<OverridableBlockListModel>(blockProperty.Alias);
                        if (childBlocks == null) { childBlocks = blockProperty.Value<BlockListModel>(null); }
                        var result = RecursivelyFindBlock(childBlocks!, matcher);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
    }
}
