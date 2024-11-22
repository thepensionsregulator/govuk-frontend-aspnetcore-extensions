using GovUk.Frontend.Umbraco;
using GovUk.Frontend.Umbraco.Blocks;
using Microsoft.Extensions.Options;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Services
{
    public class TprBlockViewInterceptorTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void Full_width_box_should_set_RenderWidthContainer_to_false_if_RenderWidthContainerForBlocks_enabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlock(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.RenderWidthContainer, Is.False);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Full_width_box_should_not_change_RenderWidthContainer_RenderWidthContainerForBlocks_disabled(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlock(TprBoxStyles.FullWidth, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = false }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.RenderWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Other_box_should_not_change_RenderWidthContainer(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = CreateTprBoxBlock(TprBoxStyles.Solid, renderWidthContainerInitialValue);

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.RenderWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Other_block_should_not_change_RenderWidthContainer(bool renderWidthContainerInitialValue)
        {
            // Arrange
            var blockView = new BlockViewModel
            {
                Block = UmbracoBlockGridFactory.CreateOverridableBlock("other"),
                RenderWidthContainer = renderWidthContainerInitialValue
            };

            var interceptor = new TprBoxViewInterceptor(Options.Create(new GovUkFrontendUmbracoOptions { RenderWidthContainerForBlocks = true }));

            // Act
            interceptor.InterceptBlockView(blockView);

            // Assert
            Assert.That(blockView.RenderWidthContainer, Is.EqualTo(renderWidthContainerInitialValue));
        }

        private static BlockViewModel CreateTprBoxBlock(string boxStyle, bool renderWidthContainerInitialValue)
        {
            var block = UmbracoBlockGridFactory.CreateOverridableBlock(
                                UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TprBox).Object,
                                UmbracoBlockGridFactory.CreateContentOrSettings(ElementTypeAliases.TprBoxSettings)
                                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.TprBoxStyle, boxStyle)
                                    .Object
                            );
            var blockView = new BlockViewModel
            {
                Block = block,
                RenderWidthContainer = renderWidthContainerInitialValue
            };
            return blockView;
        }
    }
}
