using System;
using System.Collections.ObjectModel;
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
        public static BlockListItem? FindBlock(this ReadOnlyCollection<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return RecursivelyFindBlock(blockList, matcher);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this ReadOnlyCollection<BlockListItem> blockList, string propertyName)
        {
            return RecursivelyFindBlock(blockList, x => x.Settings.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName);
        }

        internal static BlockListItem? RecursivelyFindBlock(this ReadOnlyCollection<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
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
                        var result = RecursivelyFindBlock(blockProperty.Value<BlockListModel>(null)!, matcher);
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
