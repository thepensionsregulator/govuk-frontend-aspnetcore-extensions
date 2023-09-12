using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace ThePensionsRegulator.Umbraco.Blocks
{
    public static class BlockModelCollectionExtensions
    {
        #region FindBlock
        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlock(this IEnumerable<OverridableBlockListItem> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return (OverridableBlockListItem?)RecursivelyFindBlock(blocks, matcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlock(this IEnumerable<OverridableBlockListItem> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlock(matcher, null);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlock(
             this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
             Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> matcher,
             IPublishedValueFallback? publishedValueFallback)
        {
            var baseBlocks = blocks.Select(x => x as IBlockReference<IPublishedElement, IPublishedElement>)
                                   .OfType<IBlockReference<IPublishedElement, IPublishedElement>>();
            Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> baseMatcher = block =>
            {
                var overridable = (IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>)block;
                return matcher(overridable);
            };
            return (IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>?)RecursivelyFindBlock(baseBlocks, baseMatcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlock(
             this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
             Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> matcher)
        {
            return blocks.FindBlock(matcher, null);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IBlockReference<IPublishedElement, IPublishedElement>? FindBlock(
             this IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blocks,
             Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> matcher,
             IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blocks, matcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IBlockReference<IPublishedElement, IPublishedElement>? FindBlock(
             this IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blocks,
             Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> matcher)
        {
            return blocks.FindBlock(matcher, null);
        }

        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListItem> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blocks, matcher, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first matching block in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListItem> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlock(matcher, null);
        }

        /// <summary>
        /// Recursively find the first matching block in a set of block lists and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block lists to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListModel> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            foreach (var blockList in blocks)
            {
                var block = RecursivelyFindBlock(blockList, matcher, publishedValueFallback);
                if (block != null) { return block; }
            }
            return null;
        }

        /// <summary>
        /// Recursively find the first matching block in a set of block lists and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block lists to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlock(this IEnumerable<BlockListModel> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlock(matcher, null);
        }
        #endregion

        #region FindBlocks
        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlocks<OverridableBlockListItem>(blocks, matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocks(this IEnumerable<OverridableBlockListItem> blocks)
        {
            return blocks.FindBlocks(x => true, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocks(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> matcher,
            IPublishedValueFallback? publishedValueFallback)
        {
            var baseBlocks = blocks.Select(x => x as IBlockReference<IPublishedElement, IPublishedElement>)
                .OfType<IBlockReference<IPublishedElement, IPublishedElement>>();
            Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> baseMatcher = block =>
            {
                var overridable = (IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>)block;
                return matcher(overridable);
            };
            return RecursivelyFindBlocks(baseBlocks, baseMatcher, false, publishedValueFallback).Select(block => (IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>)block);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocks(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
            Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool> matcher)
        {
            return blocks.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocks(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks)
        {
            return blocks.FindBlocks(x => true, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> FindBlocks(
            this IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blocks,
            Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> matcher,
            IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlocks(blocks, matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> FindBlocks(
            this IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blocks,
            Func<IBlockReference<IPublishedElement, IPublishedElement>, bool> matcher)
        {
            return blocks.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a set of blocks and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> FindBlocks(
            this IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>> blocks)
        {
            return blocks.FindBlocks(x => true, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlocks(blocks, matcher, false, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the matching blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a block list and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListItem> blocks)
        {
            return blocks.FindBlocks(x => true, null);
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of block lists and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block lists to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blocks, Func<BlockListItem, bool> matcher, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.SelectMany(x => x.FindBlocks(matcher, publishedValueFallback));
        }

        /// <summary>
        /// Recursively find the matching blocks in a set of block lists and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block lists to search.</param>
        /// <param name="matcher">A function which returns <c>true</c> for a matching block and <c>false</c> otherwise.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blocks, Func<BlockListItem, bool> matcher)
        {
            return blocks.FindBlocks(matcher, null);
        }

        /// <summary>
        /// Recursively find all blocks in a set of block lists and any decendant block lists.
        /// </summary>
        /// <param name="blocks">The block lists to search.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<BlockListItem> FindBlocks(this IEnumerable<BlockListModel> blocks)
        {
            return blocks.FindBlocks(x => true, null);
        }
        #endregion FindBlocks

        #region FindBlockByContentTypeAlias
        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlockByContentTypeAlias(this IEnumerable<OverridableBlockListItem> blocks, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blocks, x => x.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlockByContentTypeAlias(this IEnumerable<OverridableBlockListItem> blocks, string alias)
        {
            return blocks.FindBlockByContentTypeAlias(alias, null);
        }

        /// <summary>
        /// Recursively find the first block in a block grid and any decendant block lists where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByContentTypeAlias(this OverridableBlockGridModel blocks, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlock((IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block) => block.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block grid and any decendant block lists where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByContentTypeAlias(this OverridableBlockGridModel blocks, string alias)
        {
            return blocks.FindBlock((IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block) => block.Content.ContentType.Alias == alias);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListItem> blocks, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return RecursivelyFindBlock(blocks, x => x.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The block list to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListItem> blocks, string alias)
        {
            return blocks.FindBlockByContentTypeAlias(alias, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists where the content is of a given element type.
        /// </summary>
        /// <param name="blockCollections">The block lists to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListModel> blockCollections, string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return blockCollections.FindBlocks().FindBlockByContentTypeAlias(alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists where the content is of a given element type.
        /// </summary>
        /// <param name="blockCollections">The block lists to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static BlockListItem? FindBlockByContentTypeAlias(this IEnumerable<BlockListModel> blockCollections, string alias)
        {
            return blockCollections.FindBlocks().FindBlockByContentTypeAlias(alias, null);
        }
        #endregion

        #region FindBlocksByContentTypeAlias
        /// <summary>
        /// Recursively find the first block in a set of blocks where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByContentTypeAlias(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
            string alias, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlocks(x => x.Content.ContentType.Alias == alias, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks where the content is of a given element type.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="alias">The alias of element type for the content of the block.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByContentTypeAlias(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks,
            string alias)
        {
            return blocks.FindBlocks(x => x.Content.ContentType.Alias == alias, null);
        }
        #endregion

        #region Private methods: RecursivelyFindBlock and RecursivelyFindBlocks
        private static T? RecursivelyFindBlock<T>(IEnumerable<T> blocks, Func<T, bool> matcher, IPublishedValueFallback? publishedValueFallback) where T :
            IBlockReference<IPublishedElement, IPublishedElement>
        {
            return RecursivelyFindBlocks(blocks, matcher, true, publishedValueFallback).FirstOrDefault();
        }

        private static IList<T> RecursivelyFindBlocks<T>(IEnumerable<T> blocks, Func<T, bool> matcher, bool returnFirstMatchOnly, IPublishedValueFallback? publishedValueFallback) where T :
            IBlockReference<IPublishedElement, IPublishedElement>
        {
            if (blocks is null)
            {
                throw new ArgumentNullException(nameof(blocks));
            }

            var matchedBlocks = new List<T>();

            foreach (var block in blocks)
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
        #endregion
    }
}
