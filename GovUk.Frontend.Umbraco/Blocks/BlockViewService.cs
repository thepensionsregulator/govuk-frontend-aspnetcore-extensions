using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.Blocks;

namespace GovUk.Frontend.Umbraco.Blocks
{
    public class BlockViewService(IGovUkGridClassBuilder _gridClassBuilder, IGovUkFieldsetErrorFinder _fieldsetErrorFinder)
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
        public IEnumerable<BlockViewModel> PrepareBlockViewModels(IEnumerable<BlockGridItem> model, ModelStateDictionary modelState)
        {
            var blocksToReturn = new List<BlockViewModel>();
            var gridModel = model as OverridableBlockGridModel;
            var areaModel = model as OverridableBlockGridArea;
            var blocks = (gridModel?.FilteredBlocks() ?? areaModel?.FilteredBlocks() ?? new OverridableBlockGridModel(model, null)).ToList();
            if (!blocks.Any()) { return blocksToReturn; }

            string? previousRowClass = null, previousColumnClass = null;
            bool? previousIsGridAreasBlock = null;
            var childColumnsDefaultToFullWidth = areaModel != null || (gridModel?.ChildColumnsDefaultToFullWidth ?? false);

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

                var sameAsPrevious = (hasGridAreas == previousIsGridAreasBlock &&
                                      rowClass == HtmlClassNames.Row && previousRowClass == HtmlClassNames.Row &&
                                      columnClass == previousColumnClass);

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

                    var nextIsGridAreasBlock = (blocks[i + 1].Areas.Any());
                    sameAsNext = (hasGridAreas == nextIsGridAreasBlock &&
                                  rowClass == HtmlClassNames.Row && nextRowClass == HtmlClassNames.Row &&
                                  columnClass == nextColumnClass);
                }

                blocksToReturn.Add(new BlockViewModel
                {
                    Block = blocks[i],
                    ColumnClasses = columnClass,
                    RowClasses = rowClass,
                    HasGridAreas = hasGridAreas,
                    IsGridRow = false,
                    RenderGrid = false,
                    IsSameAsNext = sameAsNext,
                    IsSameAsPrevious = sameAsPrevious,
                    FieldsetErrorClasses = FieldsetErrorClassesForBlock(_fieldsetErrorFinder, modelState, blocks[i])
                });

                previousIsGridAreasBlock = hasGridAreas;
                previousRowClass = rowClass;
                previousColumnClass = columnClass;
            }

            return blocksToReturn;
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
                    fieldsetErrorClasses = "govuk-form-group govuk-form-group--error";
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
        public IEnumerable<BlockViewModel> PrepareBlockViewModels(IEnumerable<BlockListItem> model, ModelStateDictionary modelState)
        {
            var blocksToReturn = new List<BlockViewModel>();
            var filteredModel = model as OverridableBlockListModel ?? new OverridableBlockListModel(model, null);
            var blocks = filteredModel.FilteredBlocks().ToList();
            if (!blocks.Any()) { return blocksToReturn; }
            string? previousRowClass = null, previousColumnClass = null;
            bool? previousIsGridRowBlock = null;

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

                var sameAsPrevious = (isGridRowBlock == previousIsGridRowBlock &&
                                      rowClass == HtmlClassNames.Row && previousRowClass == HtmlClassNames.Row &&
                                      columnClass == previousColumnClass);

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

                    sameAsNext = (isGridRowBlock == (blocks[i + 1].Content.ContentType.Alias == ElementTypeAliases.GridRow) &&
                                  rowClass == HtmlClassNames.Row && nextRowClass == HtmlClassNames.Row &&
                                  columnClass == nextColumnClass);
                }

                blocksToReturn.Add(new BlockViewModel
                {
                    Block = blocks[i],
                    ColumnClasses = columnClass,
                    RowClasses = rowClass,
                    HasGridAreas = false,
                    IsGridRow = isGridRowBlock,
                    RenderGrid = filteredModel.RenderGrid,
                    IsSameAsNext = sameAsNext,
                    IsSameAsPrevious = sameAsPrevious,
                    FieldsetErrorClasses = FieldsetErrorClassesForBlock(_fieldsetErrorFinder, modelState, blocks[i])
                });

                previousIsGridRowBlock = isGridRowBlock;
                previousRowClass = rowClass;
                previousColumnClass = columnClass;
            }

            return blocksToReturn;
        }
    }
}
