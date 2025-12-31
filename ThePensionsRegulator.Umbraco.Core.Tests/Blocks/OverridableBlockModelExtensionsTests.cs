using Moq;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Core.Tests.Blocks
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
        [Fact]
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
            Assert.Equal(blockList.First(), result);
        }

        [Fact]
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
            Assert.Equal(blockGrid.First(), result);
        }

        [Fact]
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
            Assert.Equal(grandChildBlockList.First(), result);
        }

        [Fact]
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
            Assert.Equal(grandChildBlockList.First(), result);
        }

        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal(BLOCK_ALIAS, result.Content.ContentType.Alias);
        }

        [Fact]
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
            Assert.NotNull(result);
            Assert.Equal(BLOCK_ALIAS, result.Content.ContentType.Alias);
        }

        [Fact]
        public void Block_is_matched_in_multiple_OverridableBlockListModels()
        {
            var blockList1 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();
            var blockList2 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var result = (new[] { blockList1.BlockList, blockList2.BlockList }).FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.Equal(blockList1.BlocksToMatch[0], result);
        }

        [Fact]
        public void FindBlock_on_OverridableBlockListModel_returns_overridden_value_without_casting()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoContentFactory.CreateContent<IOverridablePublishedElement>()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, ORIGINAL_VALUE)
                    .Object
                    )
                );

            blockList[0].Content.OverrideValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, OVERRIDDEN_VALUE);

            var result = blockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(OVERRIDDEN_VALUE, result.Content.Value<string>(EXAMPLE_TEXTBOX_PROPERTY_ALIAS));
        }

        [Fact]
        public void FindBlock_on_OverridableBlockListModel_matches_on_overridden_value()
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoContentFactory.CreateContent<IOverridablePublishedElement>()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, ORIGINAL_VALUE)
                    .Object
                    )
                );

            blockList[0].Content.OverrideValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, OVERRIDDEN_VALUE);

            var result = blockList.FindBlock(x => x.Content.Value<string>(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) == OVERRIDDEN_VALUE);

            // Assert
            Assert.NotNull(result);
        }

        #endregion

        #region FindBlocks

        [Fact]
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_OverridableBlockGridModel()
        {
            var blockGrid = CreateOverridableBlockGridHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = blockGrid.BlockGrid.FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.Equal(2, results.Count());
            Assert.Contains(blockGrid.BlocksToMatch[0], results);
            Assert.Contains(blockGrid.BlocksToMatch[1], results);
        }

        [Fact]
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_OverridableBlockListModel()
        {
            var blockList = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = blockList.BlockList.FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.Equal(2, results.Count());
            Assert.Contains(blockList.BlocksToMatch[0], results);
            Assert.Contains(blockList.BlocksToMatch[1], results);
        }

        [Fact]
        public void Multiple_matching_blocks_are_matched_in_multiple_OverridableBlockListModels()
        {
            var blockList1 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();
            var blockList2 = CreateOverridableBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = (new[] { blockList1.BlockList, blockList2.BlockList }).FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.Equal(4, results.Count());
            Assert.Contains(blockList1.BlocksToMatch[0], results);
            Assert.Contains(blockList1.BlocksToMatch[1], results);
            Assert.Contains(blockList2.BlocksToMatch[0], results);
            Assert.Contains(blockList2.BlocksToMatch[1], results);
        }

        #endregion

        #region FindBlockByContentTypeAlias
        [Fact]
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
            Assert.Equal(blockList.First(), result);
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
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "contentTypeAlias", "value"));

            var matchingBlock1 = new OverridableBlockListItem(
                new BlockListItem(Guid.NewGuid(), matchingBlockContent1.Object, null, null),
                            OverridableBlockListItem.NoopPublishedElementFactory
                        );
            var matchingBlockContent2 = new Mock<IOverridablePublishedElement>();
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "contentTypeAlias", "value"));

            var matchingBlock2 = new OverridableBlockListItem(
                        new BlockListItem(Guid.NewGuid(), matchingBlockContent1.Object, null, null),
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
