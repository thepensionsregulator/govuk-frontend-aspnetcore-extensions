using Moq;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.BlockLists;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests
{
    public class OverridableBlockListModelTests
    {
        [Test]
        public void BlockListModels_are_converted_to_OverridableBlockListModels_including_nested_block_lists()
        {
            // Arrange
            var grandChildBlockList = UmbracoBlockListFactory.CreateBlockListModel(Array.Empty<BlockListItem>());

            var childBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            var parentBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList)
                    .Object
                    )
                );

            OverridableBlockListModel? convertedChildBlockList = null, convertedGrandChildBlockList = null;

            var parentBlockListContent = new Mock<IOverridablePublishedElement>();
            parentBlockListContent.Setup(x => x.OverrideValue("childBlocks", It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedChildBlockList = value as OverridableBlockListModel;
            });
            parentBlockListContent.Setup(x => x.Properties).Returns(parentBlockList[0].Content.Properties);
            parentBlockListContent.Setup(x => x.Value<BlockListModel>("childBlocks", null, null, default, default)).Returns(childBlockList);

            var childBlockListContent = new Mock<IOverridablePublishedElement>();
            childBlockListContent.Setup(x => x.OverrideValue("grandchildBlocks", It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedGrandChildBlockList = value as OverridableBlockListModel;
            });
            childBlockListContent.Setup(x => x.Properties).Returns(childBlockList[0].Content.Properties);
            childBlockListContent.Setup(x => x.Value<BlockListModel>("grandchildBlocks", null, null, default, default)).Returns(grandChildBlockList);

            var factoryCalls = 0;
            Func<IPublishedElement?, IOverridablePublishedElement?> factory = x =>
            {
                factoryCalls++;
                switch (factoryCalls)

                {
                    case 1:
                        return parentBlockListContent.Object;
                    case 3:
                        return childBlockListContent.Object;
                    default:
                        return null;
                }
            };

            // Act
            new OverridableBlockListModel(parentBlockList, null, factory);

            // Assert
            Assert.NotNull(convertedChildBlockList);
            Assert.NotNull(convertedGrandChildBlockList);
        }

        [Test]
        public void Indexer_acts_on_unfiltered_blocks()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            blockList.Filter = blocks => Array.Empty<OverridableBlockListItem>();

            // Act + Assert
            Assert.That(() => blockList[0], Throws.Nothing);
        }

        [Test]
        public void Can_cast_to_BlockListModel()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (BlockListModel)blockList;

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }

        [Test]
        public void Can_convert_to_BlockListModel()
        {
            var converter = TypeDescriptor.GetConverter(new OverridableBlockListModel());

            Assert.That(converter.GetType(), Is.EqualTo(typeof(OverridableBlockListTypeConverter)));
        }
    }
}
