using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    [TestFixture]
    public class PublishedContentExtensionsTests
    {
        [Test]
        public void Finds_multiple_block_lists()
        {
            // Arrange
            var blockList1 = new BlockListModel(Array.Empty<BlockListItem>());
            var blockList2 = new BlockListModel(Array.Empty<BlockListItem>());

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>();
            content.SetupUmbracoBlockListPropertyValue("blockList1", blockList1);
            content.SetupUmbracoBlockListPropertyValue("blockList2", blockList2);

            // Act
            var results = content.Object.FindBlockLists().ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.Contains(blockList1, results);
            Assert.Contains(blockList2, results);
        }

        [Test]
        public void Finds_multiple_block_grids()
        {
            // Arrange
            var blockGrid1 = new BlockGridModel(Array.Empty<BlockGridItem>(), 1);
            var blockGrid2 = new BlockGridModel(Array.Empty<BlockGridItem>(), 1);

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>();
            content.SetupUmbracoBlockGridPropertyValue("blockGrid1", blockGrid1);
            content.SetupUmbracoBlockGridPropertyValue("blockGrid2", blockGrid2);

            // Act
            var results = content.Object.FindBlockGrids().ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.Contains(blockGrid1, results);
            Assert.Contains(blockGrid2, results);
        }


        [Test]
        public void Finds_mixed_block_lists_and_grids()
        {
            // Arrange
            var blockList = new BlockListModel(Array.Empty<BlockListItem>());
            var blockGrid = new BlockGridModel(Array.Empty<BlockGridItem>(), 1);

            var content = UmbracoContentFactory.CreateContent<IPublishedContent>();
            content.SetupUmbracoBlockListPropertyValue("blockList", blockList);
            content.SetupUmbracoBlockGridPropertyValue("blockGrid", blockGrid);

            // Act
            var results = content.Object.FindBlockModelCollections(null).ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.Contains(blockList, results);
            Assert.Contains(blockGrid, results);
        }
    }
}
