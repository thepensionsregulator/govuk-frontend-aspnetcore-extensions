using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using ThePensionsRegulator.Umbraco;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests.Blocks
{
    [TestFixture]
    public class BlockViewServiceTests
    {
# nullable disable
        private Mock<IGovUkGridClassBuilder> _gridClassBuilder;
        private Mock<IGovUkFieldsetErrorFinder> _fieldsetErrorFinder;
#nullable enable

        [SetUp]
        public void SetupMocks()
        {
            _gridClassBuilder = new();
            _fieldsetErrorFinder = new();
        }

        [Test]
        public void OverridableBlockGridModel_applies_filter()
        {
            // Arrange
            const string ALLOWED = "allowed";
            const string NOT_ALLOWED = "notAllowed";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(ALLOWED),
                UmbracoBlockGridFactory.CreateOverridableBlock(NOT_ALLOWED)
                ]);
            model.Filter = block => block.Content.ContentType.Alias == ALLOWED;

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CurrentBlock.Content.ContentType.Alias, Is.EqualTo(ALLOWED));
        }

        [Test]
        public void OverridableBlockAreaModel_applies_filter()
        {
            // Arrange
            const string ALLOWED = "allowed";
            const string NOT_ALLOWED = "notAllowed";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                UmbracoBlockGridFactory.CreateOverridableBlock(ALLOWED),
                UmbracoBlockGridFactory.CreateOverridableBlock(NOT_ALLOWED)
                ], "area");
            model.Filter = block => block.Content.ContentType.Alias == ALLOWED;

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CurrentBlock.Content.ContentType.Alias, Is.EqualTo(ALLOWED));
        }

        [Test]
        public void OverridableBlockListModel_applies_filter()
        {
            // Arrange
            const string ALLOWED = "allowed";
            const string NOT_ALLOWED = "notAllowed";

            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock(ALLOWED),
                UmbracoBlockListFactory.CreateOverridableBlock(NOT_ALLOWED)
                ]);
            model.Filter = block => block.Content.ContentType.Alias == ALLOWED;

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().CurrentBlock.Content.ContentType.Alias, Is.EqualTo(ALLOWED));
        }

        [Test]
        public void Grid_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Area_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void Grid_sets_previous_current_and_next_block()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock("one"),
                UmbracoBlockGridFactory.CreateOverridableBlock("two"),
                UmbracoBlockGridFactory.CreateOverridableBlock("three")
                ]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary()).ToList();

            // Assert
            Assert.That(result[0].PreviousBlock, Is.Null);
            Assert.That(result[0].CurrentBlock, Is.EqualTo(model[0]));
            Assert.That(result[0].NextBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].PreviousBlock, Is.EqualTo(model[0]));
            Assert.That(result[1].CurrentBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].NextBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].PreviousBlock, Is.EqualTo(model[1]));
            Assert.That(result[2].CurrentBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].NextBlock, Is.Null);
        }

        [Test]
        public void Area_sets_previous_current_and_next_block()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                UmbracoBlockGridFactory.CreateOverridableBlock("one"),
                UmbracoBlockGridFactory.CreateOverridableBlock("two"),
                UmbracoBlockGridFactory.CreateOverridableBlock("three")
                ], "area");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary()).ToList();

            // Assert
            Assert.That(result[0].PreviousBlock, Is.Null);
            Assert.That(result[0].CurrentBlock, Is.EqualTo(model[0]));
            Assert.That(result[0].NextBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].PreviousBlock, Is.EqualTo(model[0]));
            Assert.That(result[1].CurrentBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].NextBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].PreviousBlock, Is.EqualTo(model[1]));
            Assert.That(result[2].CurrentBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].NextBlock, Is.Null);
        }

        [Test]
        public void Grid_sets_IsInGridArea_false()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock("block")
                );

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsInGridArea, Is.False);
        }

        [Test]
        public void Area_sets_IsInGridArea_true()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea(
                UmbracoBlockGridFactory.CreateOverridableBlock("block"),
                "area"
                );

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsInGridArea, Is.True);
        }

        [Test]
        public void List_sets_IsInGridArea_false()
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock("block")
                );

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsInGridArea, Is.False);
        }

        [Test]
        public void List_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void List_sets_previous_current_and_next_block()
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock("one"),
                UmbracoBlockListFactory.CreateOverridableBlock("two"),
                UmbracoBlockListFactory.CreateOverridableBlock("three")
                ]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary()).ToList();

            // Assert
            Assert.That(result[0].PreviousBlock, Is.Null);
            Assert.That(result[0].CurrentBlock, Is.EqualTo(model[0]));
            Assert.That(result[0].NextBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].PreviousBlock, Is.EqualTo(model[0]));
            Assert.That(result[1].CurrentBlock, Is.EqualTo(model[1]));
            Assert.That(result[1].NextBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].PreviousBlock, Is.EqualTo(model[1]));
            Assert.That(result[2].CurrentBlock, Is.EqualTo(model[2]));
            Assert.That(result[2].NextBlock, Is.Null);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Grid_sets_OpenWidthContainer_to_true_if_RenderWidthContainerForBlocks_enabled_and_RenderWidthContainer_true(bool renderWidthContainerForBlocksEnabled, bool renderWidthContainer)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock("block")
                );
            model.RenderWidthContainer = renderWidthContainer;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenWidthContainer, Is.EqualTo(renderWidthContainerForBlocksEnabled && renderWidthContainer));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Grid_sets_CloseWidthContainer_to_true_if_RenderWidthContainerForBlocks_enabled_and_RenderWidthContainer_true(bool renderWidthContainerForBlocksEnabled, bool renderWidthContainer)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock("block")
                );
            model.RenderWidthContainer = renderWidthContainer;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseWidthContainer, Is.EqualTo(renderWidthContainerForBlocksEnabled && renderWidthContainer));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Area_sets_OpenWidthContainer_to_false_for_any_RenderWidthContainerForBlocks_setting(bool renderWidthContainerForBlocksEnabled)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea(
                UmbracoBlockGridFactory.CreateOverridableBlock("block"),
                "area"
                );

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenWidthContainer, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Area_sets_CloseWidthContainer_to_false_for_any_RenderWidthContainerForBlocks_setting(bool renderWidthContainerForBlocksEnabled)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea(
                UmbracoBlockGridFactory.CreateOverridableBlock("block"),
                "area"
                );

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseWidthContainer, Is.False);
        }

        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(false, true, true, true)]
        [TestCase(false, false, true, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, true, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void Lists_sets_OpenWidthContainer_to_true_if_RenderWidthContainer_enabled_for_both_site_and_block_list_and_RenderGrid_is_true_and_current_block_is_not_grid_row(
                bool renderWidthContainerForSiteEnabled,
                bool renderWidthContainerForBlockListEnabled,
                bool renderGrid,
                bool currentBlockIsGridRow)
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "block")
                );
            model.RenderWidthContainer = renderWidthContainerForBlockListEnabled;
            model.RenderGrid = renderGrid;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForSiteEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenWidthContainer, Is.EqualTo(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !currentBlockIsGridRow));
        }

        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(false, true, true, true)]
        [TestCase(false, false, true, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(false, true, false, true)]
        [TestCase(false, true, true, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void Lists_sets_CloseWidthContainer_to_true_if_RenderWidthContainer_enabled_for_both_site_and_block_list_and_RenderGrid_is_true_and_current_block_is_not_grid_row(
                bool renderWidthContainerForSiteEnabled,
                bool renderWidthContainerForBlockListEnabled,
                bool renderGrid,
                bool currentBlockIsGridRow)
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "block")
                );
            model.RenderWidthContainer = renderWidthContainerForBlockListEnabled;
            model.RenderGrid = renderGrid;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForSiteEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseWidthContainer, Is.EqualTo(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !currentBlockIsGridRow));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Grid_sets_OpenGridRowAndColumn_to_true_for_blocks_with_no_areas(bool hasGridAreas)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                    .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area")),
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                 ]);

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenGridRowAndColumn, Is.False);
            Assert.That(result.Last().OpenGridRowAndColumn, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Grid_sets_CloseGridRowAndColumn_to_true_for_blocks_with_no_areas(bool hasGridAreas)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                    .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area")),
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                 ]);

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.False);
            Assert.That(result.Last().CloseGridRowAndColumn, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Area_sets_OpenGridRowAndColumn_to_true_for_blocks_with_no_areas(bool hasGridAreas)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                    .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area")),
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias"),
                 ], "area");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenGridRowAndColumn, Is.False);
            Assert.That(result.Last().OpenGridRowAndColumn, Is.True);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Area_sets_CloseGridRowAndColumn_to_true_for_blocks_with_no_areas(bool hasGridAreas)
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                    .AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area")),
                 UmbracoBlockGridFactory.CreateOverridableBlock("alias"),
                 ], "area");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.False);
            Assert.That(result.Last().CloseGridRowAndColumn, Is.True);
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void List_sets_OpenGridRowAndColumn_based_on_RenderGrid_if_current_block_is_not_grid_row(bool renderGrid, bool currentBlockIsGridRow)
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "block")
                );
            model.RenderGrid = renderGrid;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().OpenGridRowAndColumn, Is.EqualTo(renderGrid && !currentBlockIsGridRow));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void List_sets_CloseGridRowAndColumn_based_on_RenderGrid_if_current_block_is_not_grid_row(bool renderGrid, bool currentBlockIsGridRow)
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "block")
                );
            model.RenderGrid = renderGrid;

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.EqualTo(renderGrid && !currentBlockIsGridRow));
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void Grid_applies_fieldset_error_classes_if_there_are_fieldset_errors_and_legend_is_not_page_heading(bool hasErrors, bool legendIsPageHeading, bool expectClasses)
        {
            // Arrange
            const string FIELDSET_ERROR_CLASS = $"{HtmlClassNames.FormGroup} {HtmlClassNames.FormGroupError}";

            var modelState = new ModelStateDictionary();
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoBooleanPropertyValue(PropertyAliases.FieldsetLegendIsPageHeading, legendIsPageHeading)
                        .Object
                    )
                ]);
            var errors = hasErrors ? [UmbracoBlockGridFactory.CreateOverridableBlock(ElementTypeAliases.ErrorMessage)] : Array.Empty<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            _ = _fieldsetErrorFinder.Setup(x => x.FindErrors(model.First(), modelState)).Returns(errors);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, modelState);

            // Assert
            if (expectClasses)
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.EqualTo(FIELDSET_ERROR_CLASS));
            }
            else
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.Null);
            }
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void List_applies_fieldset_error_classes_if_there_are_fieldset_errors_and_legend_is_not_page_heading(bool hasErrors, bool legendIsPageHeading, bool expectClasses)
        {
            // Arrange
            const string FIELDSET_ERROR_CLASS = $"{HtmlClassNames.FormGroup} {HtmlClassNames.FormGroupError}";

            var modelState = new ModelStateDictionary();
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoBooleanPropertyValue(PropertyAliases.FieldsetLegendIsPageHeading, legendIsPageHeading)
                        .Object
                    )
                ]);
            var errors = hasErrors ? [UmbracoBlockListFactory.CreateOverridableBlock(ElementTypeAliases.ErrorMessage)] : Array.Empty<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>();
            _ = _fieldsetErrorFinder.Setup(x => x.FindErrors(model.First(), modelState)).Returns(errors);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, modelState);

            // Assert
            if (expectClasses)
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.EqualTo(FIELDSET_ERROR_CLASS));
            }
            else
            {
                Assert.That(result.First().FieldsetErrorClasses, Is.Null);
            }
        }

        [Test]
        public void Grid_sets_row_and_column_class_are_set_from_GovUkGridClassBuilder()
        {
            // Arrange
            const string ROW_CLASS = "example-row";
            const string COLUMN_CLASS = "example-column";

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock("alias")
                ]);
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(null)).Returns(ROW_CLASS);
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, null, "alias", false)).Returns(COLUMN_CLASS);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().RowClasses, Is.EqualTo(ROW_CLASS));
            Assert.That(result.First().ColumnClasses, Is.EqualTo(COLUMN_CLASS));
        }

        [Test]
        public void List_sets_row_and_column_class_are_set_from_GovUkGridClassBuilder()
        {
            // Arrange
            const string ROW_CLASS = "example-row";
            const string COLUMN_CLASS = "example-column";

            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock("alias")
                ]);
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(null)).Returns(ROW_CLASS);
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, null, "alias", false)).Returns(COLUMN_CLASS);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().RowClasses, Is.EqualTo(ROW_CLASS));
            Assert.That(result.First().ColumnClasses, Is.EqualTo(COLUMN_CLASS));
        }

        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void Grid_matches_SameAsNext_on_grid_areas_and_row_class_and_column_class(
            bool currentBlockHasAreas,
            bool nextBlockHasAreas,
            string currentBlockRowClass,
            string nextBlockRowClass,
            string currentBlockColumnClass,
            string nextBlockColumnClass,
            bool expectSameAsNext
            )
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, nextBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, nextBlockColumnClass)
                        .Object
                    )
                 ]);

            if (currentBlockHasAreas)
            {
                model[0].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }
            if (nextBlockHasAreas)
            {
                model[1].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(nextBlockRowClass)).Returns($"{HtmlClassNames.Row} {nextBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, nextBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {nextBlockColumnClass}");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsNext, Is.EqualTo(expectSameAsNext));
            Assert.That(result.Last().IsSameAsNext, Is.False); // last one should always be false
        }

        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void Grid_matches_SameAsPrevious_on_grid_areas_and_row_class_and_column_class(
            bool previousBlockHasAreas,
            bool currentBlockHasAreas,
            string previousBlockRowClass,
            string currentBlockRowClass,
            string previousBlockColumnClass,
            string currentBlockColumnClass,
            bool expectSameAsPrevious
            )
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, previousBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, previousBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                     UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                     UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                    )
                 ]);

            if (previousBlockHasAreas)
            {
                model[0].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }
            if (currentBlockHasAreas)
            {
                model[1].AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(previousBlockRowClass)).Returns($"{HtmlClassNames.Row} {previousBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, previousBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {previousBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsPrevious, Is.False); // first one should always be false
            Assert.That(result.Last().IsSameAsPrevious, Is.EqualTo(expectSameAsPrevious));
        }



        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if one is a grid row block and the other isn't
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void List_matches_SameAsNext_on_grid_row_block_and_row_class_and_column_class(
            bool currentBlockIsGridRow,
            bool nextBlockIsGridRow,
            string currentBlockRowClass,
            string nextBlockRowClass,
            string currentBlockColumnClass,
            string nextBlockColumnClass,
            bool expectSameAsNext
            )
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                 UmbracoBlockListFactory.CreateOverridableBlock(
                     UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                     UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRowSettings : "settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockListFactory.CreateOverridableBlock(
                     UmbracoBlockListFactory.CreateContentOrSettings(nextBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                     UmbracoBlockListFactory.CreateContentOrSettings(nextBlockIsGridRow ? ElementTypeAliases.GridRowSettings : "settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, nextBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, nextBlockColumnClass)
                        .Object
                    )
                 ]);

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(nextBlockRowClass)).Returns($"{HtmlClassNames.Row} {nextBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, nextBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {nextBlockColumnClass}");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsNext, Is.EqualTo(expectSameAsNext));
            Assert.That(result.Last().IsSameAsNext, Is.False); // last one should always be false
        }

        // true if everything the same with default row classes
        [TestCase(true, true, "", "", "", "", true)]
        [TestCase(false, false, "", "", "", "", true)]

        // false if one is a grid row block and the other isn't
        [TestCase(true, false, "", "", "", "", false)]
        [TestCase(false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, "custom", "", "", "", false)]
        [TestCase(false, false, "", "custom", "", "", false)]
        [TestCase(false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, "", "", "custom", "", false)]
        [TestCase(false, false, "", "", "", "custom", false)]
        [TestCase(false, false, "", "", "custom1", "custom2", false)]
        public void List_matches_SameAsPrevious_on_grid_row_block_and_row_class_and_column_class(
            bool previousBlockIsGridRow,
            bool currentBlockIsGridRow,
            string previousBlockRowClass,
            string currentBlockRowClass,
            string previousBlockColumnClass,
            string currentBlockColumnClass,
            bool expectSameAsPrevious
            )
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([
                 UmbracoBlockListFactory.CreateOverridableBlock(
                     UmbracoBlockListFactory.CreateContentOrSettings(previousBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                     UmbracoBlockListFactory.CreateContentOrSettings(previousBlockIsGridRow ? ElementTypeAliases.GridColumnSettings : "settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, previousBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, previousBlockColumnClass)
                        .Object
                     ),
                 UmbracoBlockListFactory.CreateOverridableBlock(
                     UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                     UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRowSettings : "settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, currentBlockRowClass)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForColumn, currentBlockColumnClass)
                        .Object
                    )
                 ]);

            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(previousBlockRowClass)).Returns($"{HtmlClassNames.Row} {previousBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses(currentBlockRowClass)).Returns($"{HtmlClassNames.Row} {currentBlockRowClass}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, previousBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {previousBlockColumnClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridColumnClasses(null, null, currentBlockColumnClass, "content", false)).Returns($"{HtmlClassNames.Column} {currentBlockColumnClass}");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().IsSameAsPrevious, Is.False); // first one should always be false
            Assert.That(result.Last().IsSameAsPrevious, Is.EqualTo(expectSameAsPrevious));
        }
    }
}
