using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Blocks
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
        public static OverridableBlockListItem? FindBlock(this IEnumerable<OverridableBlockListItem> blockList, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return (OverridableBlockListItem?)RecursivelyFindBlock(blockList, matcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlock(this IEnumerable<OverridableBlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return blockList.FindBlock(matcher, null);
        }

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
            return blockList.FindBlock(matcher, null);
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
            return blockLists.FindBlock(matcher, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blockList, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.RecursivelyFindBlocks(matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return blockList.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blockList)
        {
            return blockList.FindBlocks(x => true, null);
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
            return blockList.RecursivelyFindBlocks(matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList, Func<BlockListItem, bool> matcher)
        {
            return blockList.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a block list and any decendant block lists
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blockList)
        {
            return blockList.FindBlocks(x => true, null);
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
            return blockLists.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a set of block lists and any decendant block lists
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blockLists)
        {
            return blockLists.FindBlocks(x => true, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByContentTypeAlias(this IEnumerable<OverridableBlockListItem> blockList, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return (OverridableBlockListItem?)RecursivelyFindBlock(blockList, x => x.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="alias">The alias of element type for the content of the block</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByContentTypeAlias(this IEnumerable<OverridableBlockListItem> blockList, string alias)
        {
            return blockList.FindBlockByContentTypeAlias(alias, null);
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
            return blockList.FindBlockByContentTypeAlias(alias, null);
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
            return blockList.RecursivelyFindBlocks(matcher, true, publishedValueFallback).FirstOrDefault();
        }

        private static IList<T> RecursivelyFindBlocks<T>(this IEnumerable<T> blockList, Func<BlockListItem, bool> matcher, bool returnFirstMatchOnly, IPublishedValueFallback? publishedValueFallback) where T : BlockListItem
        {
            if (blockList is null)
            {
                throw new ArgumentNullException(nameof(blockList));
            }

            var matchedBlocks = new List<T>();

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
                        IEnumerable<T>? childBlocks = (IEnumerable<T>?)((block as OverridableBlockListItem)?.Content.Value<OverridableBlockListModel>(blockProperty.Alias));
                        if (childBlocks == null) { childBlocks = (IEnumerable<T>?)blockProperty.Value<BlockListModel>(publishedValueFallback ?? new NoopPublishedValueFallback()); }
                        var result = childBlocks!.RecursivelyFindBlocks(matcher, returnFirstMatchOnly, publishedValueFallback);
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
