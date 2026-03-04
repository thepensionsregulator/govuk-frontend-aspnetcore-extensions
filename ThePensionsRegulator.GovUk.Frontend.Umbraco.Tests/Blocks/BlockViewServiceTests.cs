using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Moq;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Blocks
{
    public class BlockViewServiceTests
    {
# nullable disable
        private Mock<IGovUkGridClassBuilder> _gridClassBuilder;
        private Mock<IGovUkFieldsetErrorFinder> _fieldsetErrorFinder;
#nullable enable

        public BlockViewServiceTests()
        {
            _gridClassBuilder = new();
            _fieldsetErrorFinder = new();
        }

        [Fact]
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
            Assert.Single(result);
            Assert.Equal(ALLOWED, result.First().CurrentBlock.Content.ContentType.Alias);
        }

        [Fact]
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
            Assert.Single(result);
            Assert.Equal(ALLOWED, result.First().CurrentBlock.Content.ContentType.Alias);
        }

        [Fact]
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
            Assert.Single(result);
            Assert.Equal(ALLOWED, result.First().CurrentBlock.Content.ContentType.Alias);
        }

        [Fact]
        public void Grid_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridModel([]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Area_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockGridFactory.CreateOverridableBlockGridArea([], "area");

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
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
            Assert.Null(result[0].PreviousBlock);
            Assert.Equal(model[0], result[0].CurrentBlock);
            Assert.Equal(model[1], result[0].NextBlock);
            Assert.Equal(model[0], result[1].PreviousBlock);
            Assert.Equal(model[1], result[1].CurrentBlock);
            Assert.Equal(model[2], result[1].NextBlock);
            Assert.Equal(model[1], result[2].PreviousBlock);
            Assert.Equal(model[2], result[2].CurrentBlock);
            Assert.Null(result[2].NextBlock);
        }

        [Fact]
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
            Assert.Null(result[0].PreviousBlock);
            Assert.Equal(model[0], result[0].CurrentBlock);
            Assert.Equal(model[1], result[0].NextBlock);
            Assert.Equal(model[0], result[1].PreviousBlock);
            Assert.Equal(model[1], result[1].CurrentBlock);
            Assert.Equal(model[2], result[1].NextBlock);
            Assert.Equal(model[1], result[2].PreviousBlock);
            Assert.Equal(model[2], result[2].CurrentBlock);
            Assert.Null(result[2].NextBlock);
        }

        [Fact]
        public void List_with_no_blocks_returns_empty_list()
        {
            // Arrange
            var model = UmbracoBlockListFactory.CreateOverridableBlockListModel([]);

            var blockViewService = new BlockViewService(_gridClassBuilder.Object, _fieldsetErrorFinder.Object, Options.Create(new GovUkFrontendUmbracoOptions()), []);

            // Act
            var result = blockViewService.PrepareBlockViewModels(model, new ModelStateDictionary());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
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
            Assert.Null(result[0].PreviousBlock);
            Assert.Equal(model[0], result[0].CurrentBlock);
            Assert.Equal(model[1], result[0].NextBlock);
            Assert.Equal(model[0], result[1].PreviousBlock);
            Assert.Equal(model[1], result[1].CurrentBlock);
            Assert.Equal(model[2], result[1].NextBlock);
            Assert.Equal(model[1], result[2].PreviousBlock);
            Assert.Equal(model[2], result[2].CurrentBlock);
            Assert.Null(result[2].NextBlock);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
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
            Assert.Equal(renderWidthContainerForBlocksEnabled && renderWidthContainer && !sameAsPrevious, result.Last().OpenWidthContainer);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
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
            Assert.Equal(renderWidthContainerForBlocksEnabled && renderWidthContainer && !sameAsNext, result.First().CloseWidthContainer);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
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
            Assert.False(result.First().OpenWidthContainer);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
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
            Assert.False(result.First().CloseWidthContainer);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, true, true, false)]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(false, false, true, true)]
        [InlineData(true, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(false, true, false, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, false, false, true)]
        [InlineData(false, false, true, false)]
        [InlineData(false, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, false, false, false)]
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
            Assert.Equal(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !sameAsPrevious, result.Last().OpenWidthContainer);
        }

        [Theory]
        [InlineData(true, true, true, true)]
        [InlineData(true, true, true, false)]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, true, true)]
        [InlineData(false, true, true, false)]
        [InlineData(false, false, true, true)]
        [InlineData(true, false, false, true)]
        [InlineData(true, true, false, false)]
        [InlineData(false, true, false, true)]
        [InlineData(true, false, true, false)]
        [InlineData(false, false, false, true)]
        [InlineData(false, false, true, false)]
        [InlineData(false, true, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, false, false, false)]
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
            Assert.Equal(renderWidthContainerForSiteEnabled && renderWidthContainerForBlockListEnabled && renderGrid && !sameAsNext, result.First().CloseWidthContainer);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
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
            Assert.Equal(!hasGridAreas && !sameAsPrevious, result.Last().OpenGridRowAndColumn);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
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
            Assert.Equal(!hasGridAreas && !sameAsNext, result.First().CloseGridRowAndColumn);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
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
            Assert.Equal(!hasGridAreas && !sameAsPrevious, result.Last().OpenGridRowAndColumn);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
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
            Assert.Equal(!hasGridAreas && !sameAsNext, result.First().CloseGridRowAndColumn);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
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
            Assert.Equal(renderGrid && !currentBlockIsGridRow && !sameAsPrevious, result.Last().OpenGridRowAndColumn);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, true)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        [InlineData(false, false, false)]
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
            Assert.Equal(renderGrid && !currentBlockIsGridRow && !sameAsNext, result.First().CloseGridRowAndColumn);
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
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
                Assert.True(result.First().OpenFieldsetErrorContainer);
                Assert.True(result.First().CloseFieldsetErrorContainer);
                Assert.Equal(FIELDSET_ERROR_CLASS, result.First().FieldsetErrorClasses);
            }
            else
            {
                Assert.False(result.First().OpenFieldsetErrorContainer);
                Assert.False(result.First().CloseFieldsetErrorContainer);
                Assert.Null(result.First().FieldsetErrorClasses);
            }
        }

        [Theory]
        [InlineData(false, false, false)]
        [InlineData(true, false, true)]
        [InlineData(true, true, false)]
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
                Assert.True(result.First().OpenFieldsetErrorContainer);
                Assert.True(result.First().CloseFieldsetErrorContainer);
                Assert.Equal(FIELDSET_ERROR_CLASS, result.First().FieldsetErrorClasses);
            }
            else
            {
                Assert.False(result.First().OpenFieldsetErrorContainer);
                Assert.False(result.First().CloseFieldsetErrorContainer);
                Assert.Null(result.First().FieldsetErrorClasses);
            }
        }

        [Fact]
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
            Assert.Equal(ROW_CLASS, result.First().RowClasses);
            Assert.Equal(COLUMN_CLASS, result.First().ColumnClasses);
        }

        [Fact]
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
            Assert.Equal(ROW_CLASS, result.First().RowClasses);
            Assert.Equal(COLUMN_CLASS, result.First().ColumnClasses);
        }

        [Theory]

        // true if everything the same with default row classes
        [InlineData(true, true, true, true, "", "", "", "", true)]
        [InlineData(false, false, false, false, "", "", "", "", true)]

        // false if grid areas different
        [InlineData(true, false, false, false, "", "", "", "", false)]
        [InlineData(false, true, false, false, "", "", "", "", false)]

        // false if one is a grid row block and the other isn't
        [InlineData(false, false, true, false, "", "", "", "", false)]
        [InlineData(false, false, false, true, "", "", "", "", false)]

        // any custom row class should return false
        [InlineData(false, false, false, false, "custom", "", "", "", false)]
        [InlineData(false, false, false, false, "", "custom", "", "", false)]
        [InlineData(false, false, false, false, "custom", "custom", "", "", false)]
        [InlineData(false, false, false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [InlineData(false, false, false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [InlineData(false, false, false, false, "", "", "custom", "", false)]
        [InlineData(false, false, false, false, "", "", "", "custom", false)]
        [InlineData(false, false, false, false, "", "", "custom1", "custom2", false)]
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
            Assert.Equal(expectSameAsNext, result);
        }

        [Theory]

        // true if everything the same with default row classes
        [InlineData(true, true, true, true, "", "", "", "", true)]
        [InlineData(false, false, false, false, "", "", "", "", true)]

        // false if grid areas different
        [InlineData(true, false, false, false, "", "", "", "", false)]
        [InlineData(false, true, false, false, "", "", "", "", false)]

        // false if one is a grid row block and the other isn't
        [InlineData(false, false, true, false, "", "", "", "", false)]
        [InlineData(false, false, false, true, "", "", "", "", false)]

        // any custom row class should return false
        [InlineData(false, false, false, false, "custom", "", "", "", false)]
        [InlineData(false, false, false, false, "", "custom", "", "", false)]
        [InlineData(false, false, false, false, "custom", "custom", "", "", false)]
        [InlineData(false, false, false, false, "custom1", "custom2", "", "", false)]

        // true if column classes match
        [InlineData(false, false, false, false, "", "", "custom", "custom", true)]

        // false if column class different
        [InlineData(false, false, false, false, "", "", "custom", "", false)]
        [InlineData(false, false, false, false, "", "", "", "custom", false)]
        [InlineData(false, false, false, false, "", "", "custom1", "custom2", false)]
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
            Assert.Equal(expectSameAsPrevious, result);
        }
    }
}
