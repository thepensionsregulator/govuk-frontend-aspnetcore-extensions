using Moq;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    /// <remarks>
    /// Extension methods are implemented for:
    /// 
    /// - IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>> 
    ///   (typically a OverridableBlockListModel or OverridableBlockGridModel)
    ///
    /// - IEnumerable<IEnumerable<IOverridableBlockReference<IOverridablePublishedElement, IOverridablePublishedElement>>>
    ///   (typically a collection of OverridableBlockListModels and/or OverridableBlockGridModels)
    ///
    /// Only overridable types are supported because ModelsBuilder is configured to use overridable types, so the built-in types
    /// are rarely encountered and they only add complexity that is not required. 
    /// </remarks>
    public class OverridableBlockModelExtensionsTests
    {
        private const string EXAMPLE_TEXTBOX_PROPERTY_ALIAS = "MyTextProperty";
        private const string ORIGINAL_VALUE = "original";
        private const string OVERRIDDEN_VALUE = "overridden";

        #region FindBlock
        [Test]
        public void Block_is_matched_in_root_OverridableBlockListModel()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

            // Act
            var result = blockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(blockList.First()));
        }

        [Test]
        public void Block_is_matched_in_root_OverridableBlockGridModel()
        {
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

            // Act
            var result = blockGrid.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(blockGrid.First()));
        }

        [Test]
        public void Block_is_matched_in_block_list_descendant_of_OverridableBlockListModel()
        {
            var grandChildBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

            var childBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            var parentBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList)
                    .Object
                )
            );

            // Act
            var result = parentBlockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(grandChildBlockList.First()));
        }

        [Test]
        public void Block_is_matched_in_block_list_descendant_of_OverridableBlockGridModel()
        {
            var grandChildBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

            var childBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            var parentBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList)
                    .Object
                )
            );

            // Act
            var result = parentBlockGrid.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(grandChildBlockList.First()));
        }

        [Test]
        public void Block_is_matched_in_area_of_OverridableBlockGridModel()
        {
            // Arrange
            const string BLOCK_ALIAS = "targetBlock";

            var formatter = Mock.Of<IPropertyValueFormatter>();
            var block = UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                );
            block.Areas = new List<OverridableBlockGridArea> {
                new OverridableBlockGridArea(new []
                {
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings(BLOCK_ALIAS).Object
                    )
                }, "area", 1,1)
            };

            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(block);

            // Act
            var result = blockGrid.FindBlock(block => block.Content.ContentType.Alias == BLOCK_ALIAS);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.ContentType.Alias, Is.EqualTo(BLOCK_ALIAS));
        }

        [Test]
        public void Block_is_matched_in_descendant_block_list_in_area_of_OverridableBlockGridModel()
        {
            // Arrange
            const string BLOCK_ALIAS = "targetBlock";
            const string CHILD_BLOCK_LIST_ALIAS = "childBlocks";

            var formatter = Mock.Of<IPropertyValueFormatter>();
            var block = UmbracoBlockGridFactory.CreateOverridableBlock(
                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                );
            block.Areas = new List<OverridableBlockGridArea> {
                new OverridableBlockGridArea(new []
                {
                    UmbracoBlockGridFactory.CreateOverridableBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings("alias")
                        .SetupUmbracoBlockListPropertyValue(CHILD_BLOCK_LIST_ALIAS,
                            UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings(BLOCK_ALIAS).Object
                                )
                            )
                        )
                        .Object
                    )
                }, "area", 1,1)
            };

            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(block);

            // Act
            var result = blockGrid.FindBlock(block => block.Content.ContentType.Alias == BLOCK_ALIAS);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.ContentType.Alias, Is.EqualTo(BLOCK_ALIAS));
        }

        [Test]
        public void Block_is_matched_in_multiple_OverridableBlockListModels()
        {
            var blockList1 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();
            var blockList2 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var result = (new[] { blockList1.BlockList, blockList2.BlockList }).FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(blockList1.BlocksToMatch[0]));
        }

        [Test]
        public void FindBlock_on_OverridableBlockListModel_returns_overridden_value_without_casting()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    new OverridablePublishedElement(
                        UmbracoContentFactory.CreateContent<IPublishedElement>()
                        .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, ORIGINAL_VALUE)
                        .Object
                        )
                    )
                );

            blockList[0].Content.OverrideValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, OVERRIDDEN_VALUE);

            var result = blockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content.Value<string>(EXAMPLE_TEXTBOX_PROPERTY_ALIAS), Is.EqualTo(OVERRIDDEN_VALUE));
        }

        [Test]
        public void FindBlock_on_OverridableBlockListModel_matches_on_overridden_value()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    new OverridablePublishedElement(
                        UmbracoContentFactory.CreateContent<IPublishedElement>()
                        .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, ORIGINAL_VALUE)
                        .Object
                        )
                    )
                );

            blockList[0].Content.OverrideValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, OVERRIDDEN_VALUE);

            var result = blockList.FindBlock(x => x.Content.Value<string>(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) == OVERRIDDEN_VALUE);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        #endregion

        #region FindBlocks

        [Test]
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_OverridableBlockGridModel()
        {
            var blockGrid = CreateOverridableBlockGridHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = blockGrid.BlockGrid.FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(blockGrid.BlocksToMatch[0], results);
            Assert.Contains(blockGrid.BlocksToMatch[1], results);
        }

        [Test]
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_OverridableBlockListModel()
        {
            var blockList = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = blockList.BlockList.FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(blockList.BlocksToMatch[0], results);
            Assert.Contains(blockList.BlocksToMatch[1], results);
        }

        [Test]
        public void Multiple_matching_blocks_are_matched_in_multiple_OverridableBlockListModels()
        {
            var blockList1 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();
            var blockList2 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = (new[] { blockList1.BlockList, blockList2.BlockList }).FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(4));
            Assert.Contains(blockList1.BlocksToMatch[0], results);
            Assert.Contains(blockList1.BlocksToMatch[1], results);
            Assert.Contains(blockList2.BlocksToMatch[0], results);
            Assert.Contains(blockList2.BlocksToMatch[1], results);
        }

        #endregion

        #region FindBlockByContentTypeAlias
        [Test]
        public void Block_is_matched_by_content_type_alias_in_OverridableBlockListModel()
        {
            var contentType = new Mock<IPublishedContentType>();
            contentType.Setup(x => x.Alias).Returns("myAlias");
            var blockContent = new Mock<IOverridablePublishedElement>();
            blockContent.SetupGet(x => x.ContentType).Returns(contentType.Object);

            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(blockContent.Object)
            );

            // Act
            var result = blockList.FindBlockByContentTypeAlias("myAlias");

            // Assert
            Assert.That(result, Is.EqualTo(blockList.First()));
        }
        #endregion

        #region Helpers

        private static (OverridableBlockListModel BlockList, IList<OverridableBlockListItem> BlocksToMatch) CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks()
        {
            var childBlockList = CreateOverridableChildBlockListWithMultipleMatchingBlocks();

            var parentBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                    UmbracoBlockListFactory.CreateOverridableBlock(
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList.BlockList)
                        .Object
                        )
                    );

            return (parentBlockList, childBlockList.BlocksToMatch);
        }

        private static (OverridableBlockGridModel BlockGrid, IList<OverridableBlockListItem> BlocksToMatch) CreateOverridableBlockGridHierarchyWithMultipleMatchingBlocks()
        {
            var childBlockList = CreateOverridableChildBlockListWithMultipleMatchingBlocks();

            var parentBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                    UmbracoBlockGridFactory.CreateBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings()
                        .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList.BlockList)
                        .Object
                        )
                    );

            return (parentBlockGrid, childBlockList.BlocksToMatch);
        }

        private static (OverridableBlockListModel BlockList, IList<OverridableBlockListItem> BlocksToMatch) CreateOverridableChildBlockListWithMultipleMatchingBlocks()
        {
            var matchingBlockContent1 = new Mock<IOverridablePublishedElement>();
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value"));

            var matchingBlock1 = new OverridableBlockListItem(
#nullable disable            
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), matchingBlockContent1.Object, null, null),
#nullable enable
                            OverridableBlockListItem.NoopPublishedElementFactory
                        );
            var matchingBlockContent2 = new Mock<IOverridablePublishedElement>();
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value"));

            var matchingBlock2 = new OverridableBlockListItem(
#nullable disable            
                        new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), matchingBlockContent1.Object, null, null),
#nullable enable
                        OverridableBlockListItem.NoopPublishedElementFactory
                    );
            var grandChildBlockList = new OverridableBlockListModel(new[] { matchingBlock1, matchingBlock2 }, null, OverridableBlockListItem.NoopPublishedElementFactory);

            var childBlockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            return (childBlockList, new List<OverridableBlockListItem> { matchingBlock1, matchingBlock2 });
        }
        #endregion
    }
}
