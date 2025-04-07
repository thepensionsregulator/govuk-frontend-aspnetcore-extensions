using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprBoxViewInterceptorTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Full_width_box_should_not_render_width_container_if_RenderWidthContainerForBlocks_enabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.OpenWidthContainer, Is.False);
            Assert.That(blockView.CloseWidthContainer, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Full_width_box_should_not_change_whether_to_render_width_container_if_RenderWidthContainerForBlocks_disabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = false }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.OpenWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
            Assert.That(blockView.CloseWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Other_box_should_not_change_whether_to_render_width_container(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlockView(TprBoxStyles.Solid, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.OpenWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
            Assert.That(blockView.CloseWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        [TestCase(true)]
        [TestCase(false)]
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
            Assert.That(blockView.OpenWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
            Assert.That(blockView.CloseWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        [TestCase(false, false, false, true)]
        [TestCase(true, false, false, true)]
        [TestCase(false, true, false, true)]
        [TestCase(false, false, true, true)]
        [TestCase(true, true, false, true)]
        [TestCase(true, false, true, true)]
        [TestCase(false, true, true, true)]

        [TestCase(false, false, false, false)]
        [TestCase(true, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(true, true, false, false)]
        [TestCase(true, false, true, false)]
        [TestCase(false, true, true, false)]
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
                Assert.That(blockViewWidthContainer.OpenWidthContainer, Is.EqualTo(!currentIsBox));
                Assert.That(blockViewNoWidthContainer.OpenWidthContainer, Is.EqualTo(previousIsBox && !currentIsBox));
                Assert.That(blockViewWidthContainer.CloseWidthContainer, Is.EqualTo(!currentIsBox));
                Assert.That(blockViewNoWidthContainer.CloseWidthContainer, Is.EqualTo(!currentIsBox && nextIsBox));
            }

            // Assert - should not be updated
            if (!boxesAreFullWidth)
            {
                Assert.That(blockViewWidthContainer.OpenWidthContainer, Is.True);
                Assert.That(blockViewWidthContainer.CloseWidthContainer, Is.True);
                Assert.That(blockViewNoWidthContainer.OpenWidthContainer, Is.False);
                Assert.That(blockViewNoWidthContainer.CloseWidthContainer, Is.False);
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
