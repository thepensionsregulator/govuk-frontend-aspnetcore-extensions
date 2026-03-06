using Moq;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.PublishedModels;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Blocks
{
    public class PublishedContentModelExtensionsTests
    {
        [Fact]
        public void If_PageHeading_block_has_text_PageHeadingOrName_returns_text()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.PageHeading)
                        .SetupUmbracoTextboxPropertyValue(PropertyAliases.PageHeading, "Custom")
                        .Object
                    )
                );

            var testContext = new UmbracoTestContext();
            testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
            testContext.CurrentPage.SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);
            var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

            // Act
            var result = model.PageHeadingOrName();

            // Assert
            Assert.Equal("Custom", result);
        }

        [Fact]
        public void If_PageHeading_block_has_no_text_PageHeadingOrName_returns_name()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(ElementTypeAliases.PageHeading)
                        .Object
                    )
                );

            var testContext = new UmbracoTestContext();
            testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
            testContext.CurrentPage.SetupUmbracoBlockListPropertyValue(nameof(ExampleModelsBuilderModel.BlockList), blockList);
            var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

            // Act
            var result = model.PageHeadingOrName();

            // Assert
            Assert.Equal("Page name", result);
        }

        [Fact]
        public void If_no_PageHeading_block_PageHeadingOrName_returns_name()
        {
            var testContext = new UmbracoTestContext();
            testContext.CurrentPage.Setup(page => page.Name).Returns("Page name");
            var model = new ExampleModelsBuilderModel(testContext.CurrentPage.Object, Mock.Of<IPublishedValueFallback>());

            // Act
            var result = model.PageHeadingOrName();

            // Assert
            Assert.Equal("Page name", result);
        }
    }
}
