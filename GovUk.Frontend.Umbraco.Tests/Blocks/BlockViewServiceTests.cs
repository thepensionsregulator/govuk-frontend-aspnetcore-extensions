using GovUk.Frontend.Umbraco.Blocks;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
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

        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Grid_sets_OpenWidthContainer_to_true_if_RenderWidthContainerForBlocks_enabled_and_RenderWidthContainer_true_and_block_is_not_the_same_as_the_previous_block(bool renderWidthContainerForBlocksEnabled, bool renderWidthContainer, bool sameAsPrevious)
        {
            // Arrange
            var model = new BlockGridViewModel(
                UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "previous")
                        .Object
                    ),
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    )
                ]))
            {
                RenderWidthContainer = renderWidthContainer
            };

            var previousBlockRowClass = sameAsPrevious ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("previous")).Returns($"{GovUkClassNames.Row}{previousBlockRowClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Last().OpenWidthContainer, Is.EqualTo(renderWidthContainerForBlocksEnabled && renderWidthContainer && !sameAsPrevious));
        }

        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void Grid_sets_CloseWidthContainer_to_true_if_RenderWidthContainerForBlocks_enabled_and_RenderWidthContainer_true_and_block_is_not_the_same_as_the_next_block(bool renderWidthContainerForBlocksEnabled, bool renderWidthContainer, bool sameAsNext)
        {
            // Arrange
            var model =
                new BlockGridViewModel(
                UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                 UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    ),
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "next")
                        .Object
                    )
                 ]))
                {
                    RenderWidthContainer = renderWidthContainer
                };

            var nextBlockRowClass = sameAsNext ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("next")).Returns($"{GovUkClassNames.Row}{nextBlockRowClass}");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForBlocksEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseWidthContainer, Is.EqualTo(renderWidthContainerForBlocksEnabled && renderWidthContainer && !sameAsNext));
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
        [TestCase(false, true, true, false)]
        [TestCase(false, false, true, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void Lists_sets_OpenWidthContainer_to_true_if_RenderWidthContainer_enabled_for_both_site_and_block_list_and_RenderGrid_is_true_and_current_block_is_not_the_same_as_the_previous_block(
                bool renderWidthContainerForSiteEnabled,
                bool renderWidthContainerForBlockListEnabled,
                bool renderGrid,
                bool sameAsPrevious)
        {
            // Arrange
            var model = new BlockListViewModel(
                UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "previous")
                        .Object
                    ),
                    UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    )
                ]))
            {
                RenderGrid = renderGrid,
                RenderWidthContainer = renderWidthContainerForBlockListEnabled
            };

            var previousBlockRowClass = sameAsPrevious ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("previous")).Returns($"{GovUkClassNames.Row}{previousBlockRowClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForSiteEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Last().OpenWidthContainer, Is.EqualTo(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !sameAsPrevious));
        }

        [TestCase(true, true, true, true)]
        [TestCase(true, true, true, false)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(false, true, true, true)]
        [TestCase(false, true, true, false)]
        [TestCase(false, false, true, true)]
        [TestCase(true, false, false, true)]
        [TestCase(true, true, false, false)]
        [TestCase(false, true, false, true)]
        [TestCase(true, false, true, false)]
        [TestCase(false, false, false, true)]
        [TestCase(false, false, true, false)]
        [TestCase(false, true, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, false, false, false)]
        public void Lists_sets_CloseWidthContainer_to_true_if_RenderWidthContainer_enabled_for_both_site_and_block_list_and_RenderGrid_is_true_and_current_block_not_the_same_as_the_next_block(
                bool renderWidthContainerForSiteEnabled,
                bool renderWidthContainerForBlockListEnabled,
                bool renderGrid,
                bool sameAsNext)
        {
            // Arrange
            var model = new BlockListViewModel(
                UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    ),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "next")
                        .Object
                    )
               ]))
            {
                RenderGrid = renderGrid,
                RenderWidthContainer = renderWidthContainerForBlockListEnabled
            };

            var nextBlockRowClass = sameAsNext ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("next")).Returns($"{GovUkClassNames.Row}{nextBlockRowClass}");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = renderWidthContainerForSiteEnabled });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseWidthContainer, Is.EqualTo(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !sameAsNext));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Grid_sets_OpenGridRowAndColumn_to_true_for_blocks_with_no_areas_and_block_is_not_the_same_as_the_previous_block(bool hasGridAreas, bool sameAsPrevious)
        {
            // Arrange
            var currentBlock = UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    );

            if (hasGridAreas)
            {
                currentBlock.AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "previous")
                        .Object
                    ),
                currentBlock
                ]);

            var previousBlockRowClass = sameAsPrevious ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("previous")).Returns($"{GovUkClassNames.Row}{previousBlockRowClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Last().OpenGridRowAndColumn, Is.EqualTo(!hasGridAreas && !sameAsPrevious));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Grid_sets_CloseGridRowAndColumn_to_true_for_blocks_with_no_areas_and_block_is_not_the_same_as_the_next_block(bool hasGridAreas, bool sameAsNext)
        {
            // Arrange
            var currentBlock = UmbracoBlockGridFactory.CreateOverridableBlock(
                   UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                   UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                       .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                       .Object
                   );

            if (hasGridAreas)
            {
                currentBlock.AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([
                    currentBlock,
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                        UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                            .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "next")
                            .Object
                    )
                  ]);

            var nextBlockRowClass = sameAsNext ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("next")).Returns($"{GovUkClassNames.Row}{nextBlockRowClass}");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.EqualTo(!hasGridAreas && !sameAsNext));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Area_sets_OpenGridRowAndColumn_to_true_for_blocks_with_no_areas_and_block_is_not_the_same_as_the_previous_block(bool hasGridAreas, bool sameAsPrevious)
        {
            // Arrange
            var currentBlock = UmbracoBlockGridFactory.CreateOverridableBlock(
                   UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                   UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                       .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                       .Object
                   );

            if (hasGridAreas)
            {
                currentBlock.AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "previous")
                        .Object
                    ),
                currentBlock
                ], "area");

            var previousBlockRowClass = sameAsPrevious ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("previous")).Returns($"{GovUkClassNames.Row}{previousBlockRowClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Last().OpenGridRowAndColumn, Is.EqualTo(!hasGridAreas && !sameAsPrevious));
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Area_sets_CloseGridRowAndColumn_to_true_for_blocks_with_no_areas_and_block_is_not_the_same_as_the_next_block(bool hasGridAreas, bool sameAsNext)
        {
            // Arrange
            var currentBlock = UmbracoBlockGridFactory.CreateOverridableBlock(
                   UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                   UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                       .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                       .Object
                   );

            if (hasGridAreas)
            {
                currentBlock.AddArea(UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area"));
            }

            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([
                    currentBlock,
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings("content").Object,
                        UmbracoBlockGridFactory.CreateContentOrSettings("settings")
                            .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "next")
                            .Object
                    )
                  ], "area");

            var nextBlockRowClass = sameAsNext ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("next")).Returns($"{GovUkClassNames.Row}{nextBlockRowClass}");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.EqualTo(!hasGridAreas && !sameAsNext));
        }

        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void List_sets_OpenGridRowAndColumn_based_on_RenderGrid_if_current_block_is_not_grid_row_and_block_is_not_the_same_as_the_previous_block(
            bool renderGrid,
            bool currentBlockIsGridRow,
            bool sameAsPrevious)
        {
            // Arrange
            var model = new BlockListViewModel(
                UmbracoBlockListFactory.CreateOverridableBlockListModel([
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "previous")
                        .Object
                    ),
                    UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    )
                ]))
            {
                RenderGrid = renderGrid
            };

            var previousBlockRowClass = sameAsPrevious ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("previous")).Returns($"{GovUkClassNames.Row}{previousBlockRowClass}");
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.Last().OpenGridRowAndColumn, Is.EqualTo(renderGrid && !currentBlockIsGridRow && !sameAsPrevious));
        }

        [TestCase(true, true, true)]
        [TestCase(true, true, false)]
        [TestCase(true, false, true)]
        [TestCase(true, false, false)]
        [TestCase(false, true, true)]
        [TestCase(false, true, false)]
        [TestCase(false, false, true)]
        [TestCase(false, false, false)]
        public void List_sets_CloseGridRowAndColumn_based_on_RenderGrid_if_current_block_is_not_grid_row_and_block_is_not_the_same_as_the_next_block(
            bool renderGrid,
            bool currentBlockIsGridRow,
            bool sameAsNext)
        {
            // Arrange
            var model = new BlockListViewModel(
               UmbracoBlockListFactory.CreateOverridableBlockListModel([
               UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(currentBlockIsGridRow ? ElementTypeAliases.GridRow : "content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "current")
                        .Object
                    ),
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings("content").Object,
                    UmbracoBlockListFactory.CreateContentOrSettings("settings")
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClassesForRow, "next")
                        .Object
                    )
              ]))
            {
                RenderGrid = renderGrid
            };

            var nextBlockRowClass = sameAsNext ? "" : " different";
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("current")).Returns($"{GovUkClassNames.Row}".TrimEnd());
            _ = _gridClassBuilder.Setup(x => x.BuildGridRowClasses("next")).Returns($"{GovUkClassNames.Row}{nextBlockRowClass}");

            var options = Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true });

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, options, []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.That(result.First().CloseGridRowAndColumn, Is.EqualTo(renderGrid && !currentBlockIsGridRow && !sameAsNext));
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void Grid_applies_fieldset_error_classes_and_container_if_there_are_fieldset_errors_and_legend_is_not_page_heading(bool hasErrors, bool legendIsPageHeading, bool expectClasses)
        {
            // Arrange
            const string FIELDSET_ERROR_CLASS = $"{GovUkClassNames.FormGroup} {GovUkClassNames.FormGroupError}";

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
                Assert.That(result.First().OpenFieldsetErrorContainer, Is.True);
                Assert.That(result.First().CloseFieldsetErrorContainer, Is.True);
                Assert.That(result.First().FieldsetErrorClasses, Is.EqualTo(FIELDSET_ERROR_CLASS));
            }
            else
            {
                Assert.That(result.First().OpenFieldsetErrorContainer, Is.False);
                Assert.That(result.First().CloseFieldsetErrorContainer, Is.False);
                Assert.That(result.First().FieldsetErrorClasses, Is.Null);
            }
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, true)]
        [TestCase(true, true, false)]
        public void List_applies_fieldset_error_classes_and_container_if_there_are_fieldset_errors_and_legend_is_not_page_heading(bool hasErrors, bool legendIsPageHeading, bool expectClasses)
        {
            // Arrange
            const string FIELDSET_ERROR_CLASS = $"{GovUkClassNames.FormGroup} {GovUkClassNames.FormGroupError}";

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
                Assert.That(result.First().OpenFieldsetErrorContainer, Is.True);
                Assert.That(result.First().CloseFieldsetErrorContainer, Is.True);
                Assert.That(result.First().FieldsetErrorClasses, Is.EqualTo(FIELDSET_ERROR_CLASS));
            }
            else
            {
                Assert.That(result.First().OpenFieldsetErrorContainer, Is.False);
                Assert.That(result.First().CloseFieldsetErrorContainer, Is.False);
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
        [TestCase(true, true, true, true, "", "", "", "", true)]
        [TestCase(false, false, false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, false, false, "", "", "", "", false)]
        [TestCase(false, true, false, false, "", "", "", "", false)]

        // false if one is a grid row block and the other isn't
        [TestCase(false, false, true, false, "", "", "", "", false)]
        [TestCase(false, false, false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, false, false, "custom", "", "", "", false)]
        [TestCase(false, false, false, false, "", "custom", "", "", false)]
        [TestCase(false, false, false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, false, false, "", "", "custom", "", false)]
        [TestCase(false, false, false, false, "", "", "", "custom", false)]
        [TestCase(false, false, false, false, "", "", "custom1", "custom2", false)]
        public void SameAsNext_matches_on_row_class_and_column_class_and_grid_areas_and_grid_row_block(
            bool currentBlockHasAreas,
            bool nextBlockHasAreas,
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

            // Act
            var result = BlockViewService.IsSameAsNext(
                                $"{GovUkClassNames.Row} {currentBlockRowClass}".TrimEnd(), $"{GovUkClassNames.Row} {nextBlockRowClass}".TrimEnd(),
                                currentBlockColumnClass, nextBlockColumnClass,
                                currentBlockHasAreas, nextBlockHasAreas,
                                currentBlockIsGridRow, nextBlockIsGridRow);

            // Assert
            Assert.That(result, Is.EqualTo(expectSameAsNext));
        }

        // true if everything the same with default row classes
        [TestCase(true, true, true, true, "", "", "", "", true)]
        [TestCase(false, false, false, false, "", "", "", "", true)]

        // false if grid areas different
        [TestCase(true, false, false, false, "", "", "", "", false)]
        [TestCase(false, true, false, false, "", "", "", "", false)]

        // false if one is a grid row block and the other isn't
        [TestCase(false, false, true, false, "", "", "", "", false)]
        [TestCase(false, false, false, true, "", "", "", "", false)]

        // any custom row class should return false
        [TestCase(false, false, false, false, "custom", "", "", "", false)]
        [TestCase(false, false, false, false, "", "custom", "", "", false)]
        [TestCase(false, false, false, false, "custom", "custom", "", "", false)]
        [TestCase(false, false, false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [TestCase(false, false, false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [TestCase(false, false, false, false, "", "", "custom", "", false)]
        [TestCase(false, false, false, false, "", "", "", "custom", false)]
        [TestCase(false, false, false, false, "", "", "custom1", "custom2", false)]
        public void SameAsPrevious_matches_on_row_class_and_column_class_and_grid_areas_and_grid_row_block(
            bool previousBlockHasAreas,
            bool currentBlockHasAreas,
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

            // Act
            var result = BlockViewService.IsSameAsPrevious(
                $"{GovUkClassNames.Row} {previousBlockRowClass}".TrimEnd(), $"{GovUkClassNames.Row} {currentBlockRowClass}".TrimEnd(),
                previousBlockColumnClass, currentBlockColumnClass,
                previousBlockHasAreas, currentBlockHasAreas,
                previousBlockIsGridRow, currentBlockIsGridRow);

            // Assert
            Assert.That(result, Is.EqualTo(expectSameAsPrevious));
        }
    }
}
