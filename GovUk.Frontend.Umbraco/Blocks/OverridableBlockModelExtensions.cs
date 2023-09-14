using System.Collections.Generic;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Blocks
{
    /// <remarks>
    /// Extension methods are implemented for:
    /// 
    /// - IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> 
    ///   (typically a IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> or OverridableBlockGridModel)
    ///
    /// - IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>>
    ///   (typically a collection of IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>s and/or OverridableBlockGridModels)
    ///
    /// Only overridable types are supported because ModelsBuilder is configured to use overridable types, so the built-in types
    /// are rarely encountered. 
    /// </remarks>
    public static class OverridableBlockModelExtensions
    {
        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>).</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByBoundProperty(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>).</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByBoundProperty(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string propertyName)
        {
            return blocks.FindBlockByBoundProperty(propertyName, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>).</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlockByBoundProperty(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string propertyName, IPublishedValueFallback? publishedValueFallback)
        {
            return (OverridableBlockListItem?)blockModels.FindBlock(x => x.Settings?.GetProperty(PropertyAliases.ModelProperty)?.GetValue()?.ToString() == propertyName, publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that is bound to a model property using the 'Model property' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="propertyName">The name of the property on the view model (use <c>nameof(model.MyProperty)</c>).</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static OverridableBlockListItem? FindBlockByBoundProperty(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string propertyName)
        {
            return blockModels.FindBlockByBoundProperty(propertyName, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridRowClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridRowClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridColumnClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of blocks and any descendant block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridColumnClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlockByClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridRowClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridRowClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlockByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridColumnClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlock(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the first block in a set of block models and any descendant block lists that has the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>? FindBlockByGridColumnClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlockByGridColumnClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks and any decendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlocks(x => x.Settings.ClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridRowClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridRowClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridColumnClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className, IPublishedValueFallback? publishedValueFallback)
        {
            return blocks.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of blocks and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blocks">The blocks to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>An IEnumerable of 0 or more matching blocks.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridColumnClass(
            this IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> blocks, string className)
        {
            return blocks.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any descendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.ClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any descendant block lists that have the expected class in the 'cssClasses' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlocksByClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any descendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridRowClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridRowClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any decendant block lists that have the expected class in the 'cssClassesForRow' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridRowClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlocksByGridRowClass(className, null);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <param name="publishedValueFallback">The published value fallback strategy.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridColumnClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className, IPublishedValueFallback? publishedValueFallback)
        {
            var blocks = new List<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            foreach (var blockList in blockModels) { blocks.AddRange(blockList); }
            return blocks.FindBlocks(x => x.Settings.GridColumnClassList().Contains(className), publishedValueFallback);
        }

        /// <summary>
        /// Recursively find the blocks in a set of block models and any decendant block lists that have the expected class in the 'cssClassesForColumn' Umbraco property in the block's settings.
        /// </summary>
        /// <param name="blockModels">The block models to search.</param>
        /// <param name="className">The name of the class to search for.</param>
        /// <returns>The first matching block, or <c>null</c> if no blocks are matched.</returns>
        public static IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> FindBlocksByGridColumnClass(
            this IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>> blockModels, string className)
        {
            return blockModels.FindBlocksByGridColumnClass(className, null);
        }
    }
}
