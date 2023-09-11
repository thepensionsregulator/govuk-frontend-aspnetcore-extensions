using System.Collections.Generic;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Blocks
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
        public static OverridableBlockListItem? FindBlockByBoundProperty(this IEnumerable<OverridableBlockListItem> blockList, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that is bound to a model property using the 'Model property' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByBoundProperty(this IEnumerable<OverridableBlockListItem> blockList, string propertyName)
        {
            return blockList.FindBlockByBoundProperty(propertyName, null);
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

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridRowClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridRowClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridColumnClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridColumnClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridRowClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridRowClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridColumnClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a block list that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridColumnClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridRowClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridRowClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridColumnClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static OverridableBlockListItem? FindBlockByGridColumnClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridRowClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridRowClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridColumnClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static BlockListItem? FindBlockByGridColumnClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.ClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridRowClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridRowClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridColumnClass(this IEnumerable<OverridableBlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridColumnClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.ClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridRowClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridRowClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridColumnClass(this IEnumerable<BlockListItem> blockList, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blockList.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockList">The block list to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>An IEnumerable of 0 or more matching blocks</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridColumnClass(this IEnumerable<BlockListItem> blockList, string className)
        {
            return blockList.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridRowClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridRowClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridColumnClass(this IEnumerable<OverridableBlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<OverridableBlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<OverridableBlockListItem> FindBlocksByGridColumnClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridRowClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridRowClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <param name="publishedValueFallback">The published value fallback strategy</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridColumnClass(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<BlockListItem>();
            foreach (var blockList in blockLists) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block lists and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings
        /// </summary>
        /// <param name="blockLists">The block lists to search</param>
        /// <param name="className">The name of the class to search for</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
        public static IEnumerable<BlockListItem> FindBlocksByGridColumnClass(this IEnumerable<BlockListModel> blockLists, string className)
        {
            return blockLists.FindBlocksByGridColumnClass(className, null);
        }
    }
}
