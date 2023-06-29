using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.BlockLists;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.BlockLists
{
    public static class BlockListModelExtensions
    {
        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListItem> blockList, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListItem> blockList, string propertyName)
        {
            return blockList.FindBlockByBoundProperty(propertyName, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListModel> blockLists, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return blockLists.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListModel> blockLists, string propertyName)
        {
            return blockLists.FindBlockByBoundProperty(propertyName, null);
        }
    }
}
