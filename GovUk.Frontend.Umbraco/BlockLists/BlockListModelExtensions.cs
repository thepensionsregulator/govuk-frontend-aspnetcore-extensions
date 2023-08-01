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
		/// <param name="className">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>)</param>
		/// <param name="publishedValueFallback">The published value fallback strategy</param>
		/// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
		public static BlockListItem? FindBlockByBoundProperty(this IEnumerable<BlockListModel> blockLists, string className, IPublishedValueFallback? publishedValueFallback)
		{
			return blockLists.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == className, publishedValueFallback);
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
			return blockList.FindBlock(x => ((OverridableBlockListItem)x).ClassList().Contains(className), publishedValueFallback);
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
			return blocks.FindBlock(x => ((OverridableBlockListItem)x).ClassList().Contains(className), publishedValueFallback);
		}

		/// <summary>
		/// Recursively find the first block in a set of block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings
		/// </summary>
		/// <param name="blockLists">The block lists to search</param>
		/// <param name="className">The name of the class to search for</param>
		/// <param name="publishedValueFallback">The published value fallback strategy</param>
		/// <returns>The first matching block, or <c>null</c> if no blocks are matched</returns>
		public static OverridableBlockListItem? FindBlockByClass(this IEnumerable<OverridableBlockListModel> blockLists, string className)
		{
			return blockLists.FindBlockByClass(className, null);
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
			return blockList.FindBlocks(x => ((OverridableBlockListItem)x).ClassList().Contains(className), publishedValueFallback);
		}

		/// <summary>
		/// Recursively find the blocks in a block list and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings
		/// </summary>
		/// <param name="blockList">The block list to search</param>
		/// <param name="className">The name of the class to search for</param>
		/// <returns>An IEnumerable of 0 or more matching blocks</returns>
		public static IEnumerable<OverridableBlockListItem> FindBlocksByClass(this IEnumerable<OverridableBlockListItem> blockList, string className)
		{
			return blockList.FindBlocks(x => ((OverridableBlockListItem)x).ClassList().Contains(className), null);
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
			return blocks.FindBlocks(x => ((OverridableBlockListItem)x).ClassList().Contains(className), publishedValueFallback);
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
	}
}
