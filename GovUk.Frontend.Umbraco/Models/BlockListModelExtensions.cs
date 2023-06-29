using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
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
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blockList, matcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return FindBlock(blockList, matcher, null);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListModel> blockLists, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            foreach (var blockList in blockLists)
            {
                var block = RecursivelyFindBlock(blockList, matcher, publishedValueFallback);
                if (block != null) { return block; }
            }
            return null;
        }

        /// <summary>
        /// Recursively find the first matching block in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListModel> blockLists, Func<BlockListItem, bool> matcher)
        {
            return FindBlock(blockLists, matcher, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlocks(blockList, matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return FindBlocks(blockList, matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList)
        {
            return FindBlocks(blockList, x => true, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blockLists, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return blockLists.SelectMany(x => x.FindBlocks(matcher, publishedValueFallback));
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blockLists, Func<BlockListItem, bool> matcher)
        {
            return FindBlocks(blockLists, matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blockLists)
        {
            return FindBlocks(blockLists, x => true, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListItem> blockList, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blockList, x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListItem> blockList, string propertyName)
        {
            return FindBlockByBoundProperty(blockList, propertyName, null);
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
            return FindBlock(blockLists, x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListModel> blockLists, string propertyName)
        {
            return FindBlockByBoundProperty(blockLists, propertyName, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListItem> blockList, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blockList, x => x.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListItem> blockList, string alias)
        {
            return FindBlockByContentTypeAlias(blockList, alias, null);
        }


        /// <summary>
        /// Recursively find the first block in a set of block lists where the content is of a given element type
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListModel> blockLists, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return blockLists.FindBlocks().FindBlockByContentTypeAlias(alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists where the content is of a given element type
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListModel> blockLists, string alias)
        {
            return blockLists.FindBlocks().FindBlockByContentTypeAlias(alias, null);
        }

        private static BlockListItem? RecursivelyFindBlock(IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlocks(blockList, matcher, true, publishedValueFallback).FirstOrDefault();
        }

        private static IList<BlockListItem> RecursivelyFindBlocks(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher, bool returnFirstMatchOnly, IPublishedValueFallback? publishedValueFallback)
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
                        if (childBlocks == null) { childBlocks = blockProperty.Value<BlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()); }
                        var result = RecursivelyFindBlocks(childBlocks!, matcher, returnFirstMatchOnly, publishedValueFallback);
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
