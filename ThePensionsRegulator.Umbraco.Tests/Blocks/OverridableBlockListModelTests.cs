using Moq;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockListModelTests
    {
        private const string DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS = "docTypeChildBlocks";
        private const string PROPERTY_ALIAS_CHILD_BLOCKS = "childBlocks";

        [SetUp]
        public void Setup()
        {
            _ = new UmbracoTestContext(); // Sets up Umbraco dependency injection
        }

        private static (OverridableBlockListModel ParentBlockList, OverridableBlockListModel ChildBlockList, OverridableBlockListModel GrandChildBlockList) CreateThreeNestedOverridableBlockLists()
        {
            var grandChildBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(Array.Empty<BlockListItem>());

            var childContent = UmbracoBlockListFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, grandChildBlockList)
                    .Object;

            var childBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(childContent)
                );

            var parentContent = UmbracoBlockListFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, childBlockList)
                    .Object;

            var parentBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(parentContent)
                );

            return (parentBlockList, childBlockList, grandChildBlockList);
        }

        [Test]
        public void BlockListModels_are_converted_to_OverridableBlockListModels_including_nested_block_lists()
        {
            // Arrange
            var grandChildBlockList = UmbracoBlockListFactory.CreateBlockListModel(Array.Empty<BlockListItem>());
            var childBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, grandChildBlockList)
                    .Object
                    )
                );
            var parentBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, childBlockList)
                    .Object
                    )
                );

            OverridableBlockListModel? convertedChildBlockList = null, convertedGrandChildBlockList = null;

            var parentBlockListContent = new Mock<IOverridablePublishedElement>();
            parentBlockListContent.Setup(x => x.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedChildBlockList = value as OverridableBlockListModel;
            });
            parentBlockListContent.Setup(x => x.Properties).Returns(parentBlockList[0].Content.Properties);
            parentBlockListContent.Setup(x => x.Value<BlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS, null, null, default, default)).Returns(childBlockList);

            var childBlockListContent = new Mock<IOverridablePublishedElement>();
            childBlockListContent.Setup(x => x.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedGrandChildBlockList = value as OverridableBlockListModel;
            });
            childBlockListContent.Setup(x => x.Properties).Returns(childBlockList[0].Content.Properties);
            childBlockListContent.Setup(x => x.Value<BlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS, null, null, default, default)).Returns(grandChildBlockList);

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
            _ = new OverridableBlockListModel(parentBlockList, null, factory);

            // Assert
            Assert.IsNotNull(convertedChildBlockList);
            Assert.IsNotNull(convertedGrandChildBlockList);

        }

        [Test]

        public void BlockListModels_are_converted_to_OverridableBlockListModels_including_overridden_nested_block_lists()
        {
            // Arrange
            const string OVERRIDDEN_BLOCK_TYPE_ALIAS = "overriddenBlock";
            const string OVERRIDDEN_TEXT_PROPERTY = "text";
            const string OVERRIDDEN_TEXT_VALUE = "this is a non-overridden property in an overridden block list";

            var parentBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    new OverridablePublishedElement(
                        UmbracoBlockListFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                            .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, UmbracoBlockListFactory.CreateOverridableBlockListModel(Array.Empty<OverridableBlockListItem>()))
                            .Object
                        )
                    )
                );

            var overriddenChildBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    new OverridablePublishedElement(
                        UmbracoBlockListFactory.CreateContentOrSettings(OVERRIDDEN_BLOCK_TYPE_ALIAS)
                            .SetupUmbracoTextboxPropertyValue(OVERRIDDEN_TEXT_PROPERTY, OVERRIDDEN_TEXT_VALUE)
                            .Object
                        )
                    )
                );
            parentBlockList[0].Content.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, overriddenChildBlockList);

            // Act
            var model = new OverridableBlockListModel(parentBlockList);

            // Assert
            var overriddenBlock = model.FindBlockByContentTypeAlias(OVERRIDDEN_BLOCK_TYPE_ALIAS);
            Assert.That(overriddenBlock, Is.Not.Null);
            Assert.That(overriddenBlock.Content.Value<string>(OVERRIDDEN_TEXT_PROPERTY), Is.EqualTo(OVERRIDDEN_TEXT_VALUE));
        }


        [Test]
        public void Filter_is_passed_down_from_constructor()
        {
            // Arrange
            var blockLists = CreateThreeNestedOverridableBlockLists();

            var filter = new Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>(block => true);

            // Act
            var model = new OverridableBlockListModel(blockLists.ParentBlockList, filter);

            // Assert
            Assert.That(model.Filter, Is.EqualTo(filter));

            var childBlockList = model[0].Content.Value<OverridableBlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(childBlockList!.Filter, Is.EqualTo(filter));

            var grandchildBlockList = childBlockList[0].Content.Value<OverridableBlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(grandchildBlockList!.Filter, Is.EqualTo(filter));
        }

        [Test]
        public void Filter_is_passed_down_from_setter()
        {
            // Arrange
            var blockLists = CreateThreeNestedOverridableBlockLists();

            var filter = new Func<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>, bool>(block => true);

            // Act
            var model = new OverridableBlockListModel(blockLists.ParentBlockList, null);
            model.Filter = filter;

            // Assert
            Assert.That(model.Filter, Is.EqualTo(filter));

            var childBlockList = model[0].Content.Value<OverridableBlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(childBlockList!.Filter, Is.EqualTo(filter));

            var grandchildBlockList = childBlockList[0].Content.Value<OverridableBlockListModel>(PROPERTY_ALIAS_CHILD_BLOCKS);
            Assert.That(grandchildBlockList!.Filter, Is.EqualTo(filter));
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

            blockList.Filter = block => false;

            // Act + Assert
            Assert.That(() => blockList[0], Throws.Nothing);
        }

        [Test]
        public void PropertyValueFormatters_are_passed_down_to_items()
        {
            // Arrange
            var formatter = Mock.Of<IPropertyValueFormatter>();
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings().Object),
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings().Object)
                ));

            // Act
            blockList.PropertyValueFormatters = new List<IPropertyValueFormatter> { formatter };

            // Assert
            Assert.That(((OverridablePublishedElement)blockList[0].Content).PropertyValueFormatters?.Count(), Is.EqualTo(1));
            Assert.That(((OverridablePublishedElement)blockList[0].Settings).PropertyValueFormatters?.Count(), Is.EqualTo(1));
        }

        [Test]
        public void PropertyValueFormatters_are_applied_when_an_OverridableBlockListModel_property_is_overridden()
        {
            // Arrange - original block list with PropertyValueFormatters, has a block with a child block list
            var formatter = new Mock<IPropertyValueFormatter>();
            formatter.Setup(x => x.IsFormatter(It.IsAny<IPublishedPropertyType>())).Returns(true);

            var parentBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoBlockListPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, new OverridableBlockListModel(Array.Empty<OverridableBlockListItem>()))
                    .Object),
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings().Object)
                ));
            parentBlockList.PropertyValueFormatters = new List<IPropertyValueFormatter> { formatter.Object };

            // Act - a property within the child block list is overridden
            const string CONTENT_PROPERTY_ALIAS_TO_OVERRIDE = "contentProperty";
            const string SETTINGS_PROPERTY_ALIAS_TO_OVERRIDE = "settingsProperty";
            const string CONTENT_PROPERTY_VALUE = "new content";
            const string SETTINGS_PROPERTY_VALUE = "new setting";

            var replacementChildBlock = UmbracoBlockListFactory.CreateOverridableBlock(
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue(CONTENT_PROPERTY_ALIAS_TO_OVERRIDE, string.Empty)
                    .Object),
                    new OverridablePublishedElement(UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoTextboxPropertyValue(SETTINGS_PROPERTY_ALIAS_TO_OVERRIDE, string.Empty)
                    .Object)
                );
            replacementChildBlock.Content.OverrideValue(CONTENT_PROPERTY_ALIAS_TO_OVERRIDE, CONTENT_PROPERTY_VALUE);
            replacementChildBlock.Settings.OverrideValue(SETTINGS_PROPERTY_ALIAS_TO_OVERRIDE, SETTINGS_PROPERTY_VALUE);

            var replacementChildBlockList = new OverridableBlockListModel(new[] { replacementChildBlock });
            parentBlockList[0].Content.OverrideValue(PROPERTY_ALIAS_CHILD_BLOCKS, replacementChildBlockList);

            // Assert
            formatter.Verify(x => x.FormatValue(CONTENT_PROPERTY_VALUE), Times.Once);
            formatter.Verify(x => x.FormatValue(SETTINGS_PROPERTY_VALUE), Times.Once);
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

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }

        [Test]
        public void Can_convert_to_BlockListModel()
        {
            var converter = TypeDescriptor.GetConverter(new OverridableBlockListModel());

            Assert.That(converter.GetType(), Is.EqualTo(typeof(OverridableBlockListTypeConverter)));
        }

        [Test]
        public void Can_cast_to_IEnumerable_of_OverridableBlockListItem()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<OverridableBlockListItem>)blockList;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }

        [Test]
        public void Can_cast_to_IEnumerable_of_BlockListItem()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<BlockListItem>)blockList;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }

        [Test]
        public void Can_cast_to_IEnumerable_of_IOverridableBlockReference()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>)blockList;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }


        [Test]
        public void Can_cast_to_IEnumerable_of_IBlockReference()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            // Act
            var model = (IEnumerable<IBlockReference<IPublishedElement, IPublishedElement>>)blockList;

            // Assert
            Assert.That(model, Is.Not.Null);
            Assert.That(model.Count, Is.EqualTo(blockList.Count()));
        }
    }
}
