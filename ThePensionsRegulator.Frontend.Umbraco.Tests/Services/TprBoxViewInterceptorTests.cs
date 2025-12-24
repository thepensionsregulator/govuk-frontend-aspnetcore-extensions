using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprBoxViewInterceptorTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Full_width_box_should_not_render_width_container_if_RenderWidthContainerForBlocks_enabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.False(blockView.OpenWidthContainer);
            Assert.False(blockView.CloseWidthContainer);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Full_width_box_should_not_change_whether_to_render_width_container_if_RenderWidthContainerForBlocks_disabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = false }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.Equal(renderWidthContainerInitialValue, blockView.OpenWidthContainer);
            Assert.Equal(renderWidthContainerInitialValue, blockView.CloseWidthContainer);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Other_box_should_not_change_whether_to_render_width_container(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.Solid, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.Equal(renderWidthContainerInitialValue, blockView.OpenWidthContainer);
            Assert.Equal(renderWidthContainerInitialValue, blockView.CloseWidthContainer);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Other_block_should_not_change_whether_to_render_width_container(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = new BlockViewModel
            {
                CurrentBlock = UmbracoBlockGridFactory.CreateOverridableBlock("other"),
                OpenWidthContainer = renderWidthContainerInitialValue,
                CloseWidthContainer = renderWidthContainerInitialValue
            };

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.Equal(renderWidthContainerInitialValue, blockView.OpenWidthContainer);
            Assert.Equal(renderWidthContainerInitialValue, blockView.CloseWidthContainer);
        }

        [Theory]
        [InlineData(false, false, false, true)]
        [InlineData(true, false, false, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, true, true)]
        [InlineData(true, true, false, true)]
        [InlineData(true, false, true, true)]
        [InlineData(false, true, true, true)]

        [InlineData(false, false, false, false)]
        [InlineData(true, false, false, false)]
        [InlineData(false, true, false, false)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(false, true, true, false)]
        public void WidthContainer_is_updated_to_reflect_whether_blocks_are_full_width_boxes(bool previousIsBox, bool currentIsBox, bool nextIsBox, bool boxesAreFullWidth)
        {
            // Arrange
            var previousBlock = previousIsBox ? CreateTprBoxBlock(boxesAreFullWidth ? TprBoxStyles.FullWidth : TprBoxStyles.Solid) : UmbracoBlockGridFactory.CreateOverridableBlock("other");
            var currentBlock = currentIsBox ? CreateTprBoxBlock(boxesAreFullWidth ? TprBoxStyles.FullWidth : TprBoxStyles.Solid) : UmbracoBlockGridFactory.CreateOverridableBlock("other");
            var nextBlock = nextIsBox ? CreateTprBoxBlock(boxesAreFullWidth ? TprBoxStyles.FullWidth : TprBoxStyles.Solid) : UmbracoBlockGridFactory.CreateOverridableBlock("other");

            var blockViewWidthContainer = new BlockViewModel
            {
                PreviousBlock = previousBlock,
                CurrentBlock = currentBlock,
                NextBlock = nextBlock,
                OpenWidthContainer = true,
                CloseWidthContainer = true,
            };

            var blockViewNoWidthContainer = new BlockViewModel
            {
                PreviousBlock = previousBlock,
                CurrentBlock = currentBlock,
                NextBlock = nextBlock,
                OpenWidthContainer = false,
                CloseWidthContainer = false
            };

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockViewWidthContainer);
            interceptor.InterceptBlockView(blockViewNoWidthContainer);

            // Assert
            if (boxesAreFullWidth)
            {
                Assert.Equal(!currentIsBox, blockViewWidthContainer.OpenWidthContainer);
                Assert.Equal(previousIsBox && !currentIsBox, blockViewNoWidthContainer.OpenWidthContainer);
                Assert.Equal(!currentIsBox, blockViewWidthContainer.CloseWidthContainer);
                Assert.Equal(!currentIsBox && nextIsBox, blockViewNoWidthContainer.CloseWidthContainer);
            }

            // Assert - should not be updated
            if (!boxesAreFullWidth)
            {
                Assert.True(blockViewWidthContainer.OpenWidthContainer);
                Assert.True(blockViewWidthContainer.CloseWidthContainer);
                Assert.False(blockViewNoWidthContainer.OpenWidthContainer);
                Assert.False(blockViewNoWidthContainer.CloseWidthContainer);
            }

        }

        private static BlockViewModel CreateTprBoxBlockView(string boxStyle, bool renderWidthContainerInitialValue)
        {
            var block = CreateTprBoxBlock(boxStyle);
            var blockView = new BlockViewModel
            {
                CurrentBlock = block,
                OpenWidthContainer = renderWidthContainerInitialValue,
                CloseWidthContainer = renderWidthContainerInitialValue
            };
            return blockView;
        }

        private static OverridableBlockGridItem CreateTprBoxBlock(string boxStyle)
        {
            return UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings(TprElementTypeAliases.Box).Object,
                        UmbracoBlockGridFactory.CreateContentOrSettings(TprElementTypeAliases.BoxSettings)
                            .SetupUmbracoTextboxPropertyValue(TprPropertyAliases.BoxStyle, boxStyle)
                            .Object
                    );
        }
    }
}
