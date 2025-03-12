using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public class BlockViewService(IGovUkGridClassBuilder _gridClassBuilder, IGovUkFieldsetErrorFinder _fieldsetErrorFinder, IOptions<GovUkFrontendUmbracoOptions> _options, IEnumerable<IBlockViewInterceptor> _interceptors)
    {
        /// <summary>
        /// Builds details of the HTML required to render each block in a block grid.
        /// </summary>
        /// <remarks>
        /// Renders blocks within a GOV.UK grid row, except where that block has areas in which case that task is delegated.
        /// Combines sibling rows without classes applied to simplify the HTML, but also to reduce instances where the grid rows
        /// interfere with the spacing between components. Spacing (particularly for inset text) can rely on margin collapsing and
        /// wrapping every component in a grid row prevents that from working because the components no longer directly follow each other.
        /// </remarks>
        public IEnumerable<BlockViewModel> PrepareBlockViewModels(IEnumerable<BlockGridItem> blockGridItems, ModelStateDictionary modelState)
        {
            var blocksToReturn = new List<BlockViewModel>();
            var wrappedModel = blockGridItems as BlockGridViewModel;
            var gridModel = blockGridItems as OverridableBlockGridModel ?? wrappedModel?.BlockGrid;
            var areaModel = blockGridItems as OverridableBlockGridArea;
            var blocks = (gridModel?.FilteredBlocks() ?? areaModel?.FilteredBlocks() ?? new OverridableBlockGridModel(blockGridItems, null)).ToList();
            if (!blocks.Any()) { return blocksToReturn; }

            string? previousRowClass = null, previousColumnClass = null;
            bool? previousHasGridAreas = null;
            var isInGridArea = areaModel is not null;
            var childColumnsDefaultToFullWidth = isInGridArea || (wrappedModel?.ChildColumnsDefaultToFullWidth ?? false);

            for (var i = 0; i < blocks.Count; i++)
            {
                if (blocks[i]?.ContentUdi == null) { continue; }

                var hasGridAreas = blocks[i].Areas.Any();
                string rowClass = _gridClassBuilder.BuildGridRowClasses(blocks[i].Settings?.Value<string>(PropertyAliases.CssClassesForRow));
                var columnClass = _gridClassBuilder.BuildGridColumnClasses(
                                    blocks[i].Settings?.Value<string>(PropertyAliases.ColumnSize),
                                    blocks[i].Settings?.Value<string>(PropertyAliases.ColumnSizeFromDesktop),
                                    blocks[i].Settings?.Value<string>(PropertyAliases.CssClassesForColumn),
                                    blocks[i].Content.ContentType.Alias,
                                    childColumnsDefaultToFullWidth);

                bool sameAsPrevious = IsSameAsPrevious(previousRowClass, rowClass, previousColumnClass, columnClass, previousHasGridAreas, hasGridAreas, false, false);

                string nextRowClass, nextColumnClass = string.Empty;
                var notTheLastBlock = i < blocks.Count - 1;
                var sameAsNext = false;

                if (notTheLastBlock)
                {
                    nextRowClass = _gridClassBuilder.BuildGridRowClasses(blocks[i + 1].Settings?.Value<string>(PropertyAliases.CssClassesForRow));
                    nextColumnClass = _gridClassBuilder.BuildGridColumnClasses(
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.ColumnSize),
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.ColumnSizeFromDesktop),
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.CssClassesForColumn),
                                                            blocks[i + 1].Content.ContentType.Alias,
                                                            childColumnsDefaultToFullWidth);

                    var nextHasGridAreas = (blocks[i + 1].Areas.Any());
                    sameAsNext = IsSameAsNext(rowClass, nextRowClass, columnClass, nextColumnClass, hasGridAreas, nextHasGridAreas, false, false);
                }

                var fieldsetErrorClasses = FieldsetErrorClassesForBlock(_fieldsetErrorFinder, modelState, blocks[i]);
                var renderFieldsetErrorContainer = !string.IsNullOrEmpty(fieldsetErrorClasses);

                var model = new BlockViewModel
                {
                    PreviousBlock = i > 0 ? blocks[i - 1] : null,
                    CurrentBlock = blocks[i],
                    NextBlock = notTheLastBlock ? blocks[i + 1] : null,
                    ColumnClasses = columnClass,
                    RowClasses = rowClass,
                    OpenGridRowAndColumn = !hasGridAreas && !sameAsPrevious,
                    CloseGridRowAndColumn = !hasGridAreas && !sameAsNext,
                    OpenWidthContainer = _options.Value.RenderWidthContainerForBlocks && !isInGridArea && (wrappedModel?.RenderWidthContainer ?? true) && !sameAsPrevious,
                    CloseWidthContainer = _options.Value.RenderWidthContainerForBlocks && !isInGridArea && (wrappedModel?.RenderWidthContainer ?? true) && !sameAsNext,
                    OpenFieldsetErrorContainer = renderFieldsetErrorContainer,
                    CloseFieldsetErrorContainer = renderFieldsetErrorContainer,
                    FieldsetErrorClasses = fieldsetErrorClasses
                };

                foreach (var interceptor in _interceptors) { interceptor.InterceptBlockView(model); }

                blocksToReturn.Add(model);

                previousHasGridAreas = hasGridAreas;
                previousRowClass = rowClass;
                previousColumnClass = columnClass;
            }

            return blocksToReturn;
        }

        internal static bool IsSameAsPrevious(
            string? previousRowClass, string currentRowClass,
            string? previousColumnClass, string currentColumnClass,
            bool? previousHasGridAreas, bool currentHasGridAreas,
            bool? previousIsGridRowBlock, bool currentIsGridRowBlock)
        {
            return (currentIsGridRowBlock == previousIsGridRowBlock &&
                    currentHasGridAreas == previousHasGridAreas &&
                    currentRowClass == GovUkClassNames.Row && previousRowClass == GovUkClassNames.Row &&
                    currentColumnClass == previousColumnClass);
        }

        internal static bool IsSameAsNext(
            string currentRowClass, string nextRowClass,
            string currentColumnClass, string nextColumnClass,
            bool currentHasGridAreas, bool? nextHasGridAreas,
            bool currentIsGridRowBlock, bool? nextIsGridRowBlock)
        {
            return (currentIsGridRowBlock == nextIsGridRowBlock &&
                    currentHasGridAreas == nextHasGridAreas &&
                    currentRowClass == GovUkClassNames.Row && nextRowClass == GovUkClassNames.Row &&
                    currentColumnClass == nextColumnClass);
        }

        private static string? FieldsetErrorClassesForBlock(IGovUkFieldsetErrorFinder _fieldsetErrorFinder, ModelStateDictionary modelState, IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement> block)
        {
            // If this block is a fieldset and there is a fieldset-level error, add extra classes to show that the entire fieldset is in an error state.
            // But only if the 'legendIsPageHeading' setting is false, otherwise it's done in GovUkFieldset.cshtml.
            var fieldsetErrors = _fieldsetErrorFinder.FindErrors(block, modelState);
            string? fieldsetErrorClasses = null;
            if (fieldsetErrors.Any())
            {
                var legendIsPageHeading = block.Settings?.Value<bool>(PropertyAliases.FieldsetLegendIsPageHeading) ?? false;
                if (!legendIsPageHeading)
                {
                    fieldsetErrorClasses = $"{GovUkClassNames.FormGroup} {GovUkClassNames.FormGroupError}";
                }
            }

            return fieldsetErrorClasses;
        }

        /// <summary>
        /// Builds details of the HTML required to render each block in a block list.
        /// </summary>
        /// <remarks>
        /// Renders blocks within a GOV.UK grid row, except where that block is a 'govukGridRow' in which case that task is delegated.
        /// Combines sibling rows without classes applied to simplify the HTML, but also to reduce instances where the grid rows 
        /// interfere with the spacing between components. Spacing (particularly for inset text) can rely on margin collapsing and
        /// wrapping every component in a grid row prevents that from working because the components no longer directly follow each other.
        /// </remarks>
        public IEnumerable<BlockViewModel> PrepareBlockViewModels(IEnumerable<BlockListItem> blockListItems, ModelStateDictionary modelState)
        {
            var blocksToReturn = new List<BlockViewModel>();
            var wrappedModel = blockListItems as BlockListViewModel;
            var filteredModel = blockListItems as OverridableBlockListModel ?? wrappedModel?.BlockList ?? new OverridableBlockListModel(blockListItems, null);
            var renderGrid = (wrappedModel?.RenderGrid ?? true);
            var blocks = filteredModel.FilteredBlocks().ToList();
            if (!blocks.Any()) { return blocksToReturn; }
            string? previousRowClass = null, previousColumnClass = null;
            bool? previousIsGridRowBlock = null, nextIsGridRowBlock = null;

            for (var i = 0; i < blocks.Count; i++)
            {
                if (blocks[i]?.ContentUdi == null) { continue; }

                var isGridRowBlock = blocks[i].Content.ContentType.Alias == ElementTypeAliases.GridRow;
                string rowClass = _gridClassBuilder.BuildGridRowClasses(blocks[i].Settings?.Value<string>(PropertyAliases.CssClassesForRow));
                var columnClass = _gridClassBuilder.BuildGridColumnClasses(
                                    blocks[i].Settings?.Value<string>(PropertyAliases.ColumnSize),
                                    blocks[i].Settings?.Value<string>(PropertyAliases.ColumnSizeFromDesktop),
                                    blocks[i].Settings?.Value<string>(PropertyAliases.CssClassesForColumn),
                                    blocks[i].Content.ContentType.Alias);

                var sameAsPrevious = IsSameAsPrevious(previousRowClass, rowClass, previousColumnClass, columnClass, false, false, previousIsGridRowBlock, isGridRowBlock);

                string nextRowClass, nextColumnClass = string.Empty;
                var notTheLastBlock = i < blocks.Count - 1;
                var sameAsNext = false;

                if (notTheLastBlock)
                {
                    nextRowClass = _gridClassBuilder.BuildGridRowClasses(blocks[i + 1].Settings?.Value<string>(PropertyAliases.CssClassesForRow));
                    nextColumnClass = _gridClassBuilder.BuildGridColumnClasses(
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.ColumnSize),
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.ColumnSizeFromDesktop),
                                                            blocks[i + 1].Settings?.Value<string>(PropertyAliases.CssClassesForColumn),
                                                            blocks[i + 1].Content.ContentType.Alias);
                    nextIsGridRowBlock = (blocks[i + 1].Content.ContentType.Alias == ElementTypeAliases.GridRow);

                    sameAsNext = IsSameAsNext(rowClass, nextRowClass, columnClass, nextColumnClass, false, false, isGridRowBlock, nextIsGridRowBlock);
                }

                var renderGridRowAndColumn = renderGrid && !isGridRowBlock;
                var fieldsetErrorClasses = FieldsetErrorClassesForBlock(_fieldsetErrorFinder, modelState, blocks[i]);
                var renderFieldsetErrorContainer = !string.IsNullOrEmpty(fieldsetErrorClasses);

                var model = new BlockViewModel
                {
                    PreviousBlock = i > 0 ? blocks[i - 1] : null,
                    CurrentBlock = blocks[i],
                    NextBlock = notTheLastBlock ? blocks[i + 1] : null,
                    ColumnClasses = columnClass,
                    RowClasses = rowClass,
                    OpenGridRowAndColumn = renderGridRowAndColumn && !sameAsPrevious,
                    CloseGridRowAndColumn = renderGridRowAndColumn && !sameAsNext,
                    OpenWidthContainer = _options.Value.RenderWidthContainerForBlocks && (wrappedModel?.RenderWidthContainer ?? true) && renderGrid && !sameAsPrevious,
                    CloseWidthContainer = _options.Value.RenderWidthContainerForBlocks && (wrappedModel?.RenderWidthContainer ?? true) && renderGrid && !sameAsNext,
                    OpenFieldsetErrorContainer = renderFieldsetErrorContainer,
                    CloseFieldsetErrorContainer = renderFieldsetErrorContainer,
                    FieldsetErrorClasses = fieldsetErrorClasses
                };

                foreach (var interceptor in _interceptors) { interceptor.InterceptBlockView(model); }

                blocksToReturn.Add(model);

                previousIsGridRowBlock = isGridRowBlock;
                previousRowClass = rowClass;
                previousColumnClass = columnClass;
            }

            return blocksToReturn;
        }
    }
}
