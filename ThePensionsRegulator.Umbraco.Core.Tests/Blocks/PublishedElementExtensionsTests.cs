using Moq;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Tests.Blocks
{

    public class PublishedElementExtensionsTests
    {
        [Fact]
        public void Finds_multiple_mixed_block_lists_and_grids()
        {
            // Arrange
            var blockList1 = new OverridableBlockListModel(Mock.Of<IPublishedValueFallback>(), new[] { UmbracoBlockListFactory.CreateOverridableBlock(UmbracoBlockListFactory.CreateContentOrSettings("alias").Object) });
            var blockList2 = new OverridableBlockListModel(Mock.Of<IPublishedValueFallback>(), new[] { UmbracoBlockListFactory.CreateOverridableBlock(UmbracoBlockListFactory.CreateContentOrSettings("alias").Object) });
            var blockGrid1 = new OverridableBlockGridModel(Mock.Of<IPublishedValueFallback>(), new[] { UmbracoBlockGridFactory.CreateOverridableBlock(UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object) });
            var blockGrid2 = new OverridableBlockGridModel(Mock.Of<IPublishedValueFallback>(), new[] { UmbracoBlockGridFactory.CreateOverridableBlock(UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object) });

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>();
            content.SetupUmbracoBlockListPropertyValue("blockList1", blockList1);
            content.SetupUmbracoBlockListPropertyValue("blockList2", blockList2);
            content.SetupUmbracoBlockGridPropertyValue("blockGrid1", blockGrid1);
            content.SetupUmbracoBlockGridPropertyValue("blockGrid2", blockGrid2);

            // Act
            var results = content.Object.FindOverridableBlockModels(null).ToList();

            // Assert
            Assert.Equal(4, results.Count);
            Assert.Contains(blockList1, results);
            Assert.Contains(blockList2, results);
            Assert.Contains(blockGrid1, results);
            Assert.Contains(blockGrid2, results);
            Assert.Single(results[0]);
            Assert.Single(results[1]);
            Assert.Single(results[2]);
            Assert.Single(results[3]);
        }
    }
}
