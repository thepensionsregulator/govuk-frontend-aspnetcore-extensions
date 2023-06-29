using ThePensionsRegulator.Umbraco.BlockLists;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests
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
            content.SetupUmbracoBlockListPropertyValue("blockList1", blockList2);

            // Act
            var results = content.Object.FindBlockLists().ToList();

            // Assert
            Assert.That(results.Count, Is.EqualTo(2));
            Assert.Contains(blockList1, results);
            Assert.Contains(blockList2, results);
        }
    }
}
