using Moq;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockGridModelTests
    {
        private const string PROPERTY_ALIAS_CHILD_BLOCKS = "childBlocks";

        private static (OverridableBlockGridModel ParentBlockGrid, OverridableBlockGridModel ChildBlockGrid, OverridableBlockGridModel GrandChildBlockGrid) CreateThreeNestedOverridableBlockGrids()
        {
            var grandChildBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(Array.Empty<BlockGridItem>());

            var childContent = UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, grandChildBlockGrid)
                    .Object;

            var childBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(childContent)
                );

            var parentContent = UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, childBlockGrid)
                    .Object;

            var parentBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(parentContent)
                );

            return (parentBlockGrid, childBlockGrid, grandChildBlockGrid);
        }

        [Test]
        public void BlockGridModels_are_converted_to_OverridableBlockGridModels_including_nested_block_grids()
        {
            // Arrange
            var grandChildBlockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(Array.Empty<BlockGridItem>());
            var childBlockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(
                UmbracoBlockGridFactory.CreateBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, grandChildBlockGrid)
                    .Object
                    )
                );
            var parentBlockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(
                UmbracoBlockGridFactory.CreateBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, childBlockGrid)
                    .Object
                    )
                );

            OverridableBlockGridModel? convertedChildBlockGrid = null, convertedGrandChildBlockGrid = null;

            var parentBlockGridContent = new Mock<IOverridablePublishedElement>();
            parentBlockGridContent.Setup(x => x.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedChildBlockGrid = value as OverridableBlockGridModel;
            });
            parentBlockGridContent.Setup(x => x.Properties).Returns(parentBlockGrid[0].Content.Properties);
            parentBlockGridContent.Setup(x => x.Value<BlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS, null, null, default, default)).Returns(childBlockGrid);

            var childBlockGridContent = new Mock<IOverridablePublishedElement>();
            childBlockGridContent.Setup(x => x.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedGrandChildBlockGrid = value as OverridableBlockGridModel;
            });
            childBlockGridContent.Setup(x => x.Properties).Returns(childBlockGrid[0].Content.Properties);
            childBlockGridContent.Setup(x => x.Value<BlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS, null, null, default, default)).Returns(grandChildBlockGrid);

            var factoryCalls = 0;
            Func<IPublishedElement?, IOverridablePublishedElement?> factory = x =>
            {
                factoryCalls++;
                switch (factoryCalls)

                {
                    case 1:
                        return parentBlockGridContent.Object;
                    case 3:
                        return childBlockGridContent.Object;
                    default:
                        return null;
                }
            };

            // Act
            _ = new OverridableBlockGridModel(parentBlockGrid, null, factory);

            // Assert
            Assert.NotNull(convertedChildBlockGrid);
            Assert.NotNull(convertedGrandChildBlockGrid);
        }

        [Test]
        public void Filter_is_passed_down_from_constructor()
        {
            // Arrange
            var blockGrids = CreateThreeNestedOverridableBlockGrids();

            var filter = new Func<OverridableBlockGridItem, bool>(block => true);

            // Act
            var model = new OverridableBlockGridModel(blockGrids.ParentBlockGrid, filter);

            // Assert
            Assert.That(model.Filter, Is.EqualTo(filter));

            var childBlockGrid = model[0].Content.Value<OverridableBlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(childBlockGrid!.Filter, Is.EqualTo(filter));

            var grandchildBlockGrid = childBlockGrid[0].Content.Value<OverridableBlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(grandchildBlockGrid!.Filter, Is.EqualTo(filter));
        }

        [Test]
        public void Filter_is_passed_down_from_setter()
        {
            // Arrange
            var blockGrids = CreateThreeNestedOverridableBlockGrids();

            var filter = new Func<OverridableBlockGridItem, bool>(block => true);

            // Act
            var model = new OverridableBlockGridModel(blockGrids.ParentBlockGrid, null);
            model.Filter = filter;

            // Assert
            Assert.That(model.Filter, Is.EqualTo(filter));

            var childBlockGrid = model[0].Content.Value<OverridableBlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(childBlockGrid!.Filter, Is.EqualTo(filter));

            var grandchildBlockGrid = childBlockGrid[0].Content.Value<OverridableBlockGridModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(grandchildBlockGrid!.Filter, Is.EqualTo(filter));
        }

        [Test]
        public void Indexer_acts_on_unfiltered_blocks()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            blockGrid.Filter = block => false;

            // Act + Assert
            Assert.That(() => blockGrid[0], Throws.Nothing);
        }

        [Test]
        public void Can_cast_to_BlockGridModel()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (BlockGridModel)blockGrid;

            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockGrid.Count()));
        }

        [Test]
        public void Can_convert_to_BlockGridModel()
        {
            var converter = TypeDescriptor.GetConverter(new OverridableBlockGridModel());

            Assert.That(converter.GetType(), Is.EqualTo(typeof(OverridableBlockGridTypeConverter)));
        }

        [Test]
        public void Can_cast_to_IEnumerable_of_OverridableBlockGridItem()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<OverridableBlockGridItem>)blockGrid;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockGrid.Count()));
        }

        [Test]
        public void Can_cast_to_IEnumerable_of_BlockGridItem()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<BlockGridItem>)blockGrid;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockGrid.Count()));
        }


        [Test]
        public void Can_cast_to_IEnumerable_of_IOverridableBlockReference()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>)blockGrid;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockGrid.Count()));
        }


        [Test]
        public void Can_cast_to_IEnumerable_of_IBlockReference()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>>)blockGrid;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockGrid.Count()));
        }
    }
}
