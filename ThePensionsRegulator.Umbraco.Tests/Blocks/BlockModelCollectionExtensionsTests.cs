using Moq;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class BlockModelCollectionExtensionsTests
    {
        private const string EXAMPLE_TEXTBOX_PROPERTY_ALIAS = "MyTextProperty";

        [Test]
        public void Block_is_matched_in_root_BlockListModel()
        {
            var blockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
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
        public void Block_is_matched_in_root_BlockGridModel()
        {
            var blockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(
                UmbracoBlockGridFactory.CreateBlock(
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
        public void Block_is_matched_in_block_list_descendant_of_BlockListModel()
        {
            var grandChildBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

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

            // Act
            var result = parentBlockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.That(result, Is.EqualTo(grandChildBlockList.First()));
        }



        [Test]
        public void Block_is_matched_in_block_list_descendant_of_BlockGridModel()
        {
            var grandChildBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value")
                    .Object
                )
            );

            var childBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            var parentBlockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(
                UmbracoBlockGridFactory.CreateBlock(
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
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_BlockListModel()
        {
            var blockList = CreateBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = blockList.BlockList.FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.Contains(blockList.BlocksToMatch[0], results);
            Assert.Contains(blockList.BlocksToMatch[1], results);
        }


        [Test]
        public void Multiple_matching_blocks_are_matched_in_block_list_descendant_of_BlockGridModel()
        {
            var blockGrid = CreateBlockGridHierarchyWithMultipleMatchingBlocks();

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

        private static (BlockListModel BlockList, IList<BlockListItem> BlocksToMatch) CreateBlockListHierarchyWithMultipleMatchingBlocks()
        {
            var childBlockList = CreateChildBlockListWithMultipleMatchingBlocks();

            var parentBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                    UmbracoBlockListFactory.CreateBlock(
                        UmbracoBlockListFactory.CreateContentOrSettings()
                        .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList.BlockList)
                        .Object
                        )
                    );

            return (parentBlockList, childBlockList.BlocksToMatch);
        }

        private static (BlockGridModel BlockGrid, IList<IBlockReference<IPublishedElement, IPublishedElement>> BlocksToMatch) CreateBlockGridHierarchyWithMultipleMatchingBlocks()
        {
            var childBlockList = CreateChildBlockListWithMultipleMatchingBlocks();

            var parentBlockGrid = UmbracoBlockGridFactory.CreateBlockGridModel(
                    UmbracoBlockGridFactory.CreateBlock(
                        UmbracoBlockGridFactory.CreateContentOrSettings()
                        .SetupUmbracoBlockListPropertyValue("childBlocks", childBlockList.BlockList)
                        .Object
                        )
                    );

            return (parentBlockGrid, childBlockList.BlocksToMatch.Select(block => block as IBlockReference<IPublishedElement, IPublishedElement>).ToList());
        }

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

        private static (BlockListModel BlockList, IList<BlockListItem> BlocksToMatch) CreateChildBlockListWithMultipleMatchingBlocks()
        {
            var matchingBlockContent1 = new Mock<IPublishedElement>();
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value"));


#nullable disable
            var matchingBlock1 = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), matchingBlockContent1.Object, null, null);
#nullable enable
            var matchingBlockContent2 = new Mock<IPublishedElement>();
            matchingBlockContent1.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value"));

#nullable disable
            var matchingBlock2 = new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), matchingBlockContent1.Object, null, null);
#nullable enable
            var grandChildBlockList = new BlockListModel(new[] { matchingBlock1, matchingBlock2 });

            var childBlockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoBlockListPropertyValue("grandchildBlocks", grandChildBlockList)
                    .Object
                    )
                );

            return (childBlockList, new List<BlockListItem> { matchingBlock1, matchingBlock2 });
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

        [Test]
        public void Multiple_matching_blocks_are_matched_in_multiple_BlockListModels()
        {
            var blockList1 = CreateBlockListHierarchyWithMultipleMatchingBlocks();
            var blockList2 = CreateBlockListHierarchyWithMultipleMatchingBlocks();

            // Act
            var results = (new[] { blockList1.BlockList, blockList2.BlockList }).FindBlocks(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null).ToList();

            // Assert
            Assert.That(results.Count(), Is.EqualTo(4));
            Assert.Contains(blockList1.BlocksToMatch[0], results);
            Assert.Contains(blockList1.BlocksToMatch[1], results);
            Assert.Contains(blockList2.BlocksToMatch[0], results);
            Assert.Contains(blockList2.BlocksToMatch[1], results);
        }

        [Test]
        public void Block_is_matched_by_content_type_alias_in_BlockListModel()
        {
            var contentType = new Mock<IPublishedContentType>();
            contentType.Setup(x => x.Alias).Returns("myAlias");
            var blockContent = new Mock<IPublishedElement>();
            blockContent.SetupGet(x => x.ContentType).Returns(contentType.Object);

            var blockList = new BlockListModel(new List<BlockListItem> {
#nullable disable            
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), blockContent.Object, null, null)
#nullable enable
            });

            // Act
            var result = blockList.FindBlockByContentTypeAlias("myAlias");

            // Assert
            Assert.That(result, Is.EqualTo(blockList.First()));
        }

        [Test]
        public void FindBlock_on_OverridableBlockListModel_returns_OverridableBlockListItem()
        {
            var childContent = new Mock<IOverridablePublishedElement>();
            childContent.Setup(x => x.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS)).Returns(UmbracoPropertyFactory.CreateTextboxProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS, "value"));

            var childBlockItem = new OverridableBlockListItem(
#nullable disable
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), childContent.Object, null, null),
#nullable enable
                OverridableBlockListItem.NoopPublishedElementFactory
            );

            var childBlockList = new OverridableBlockListModel(new[] { childBlockItem }, null, OverridableBlockListItem.NoopPublishedElementFactory);

            var parentContent = new Mock<IOverridablePublishedElement>();
            var parentProperties = new[] { UmbracoPropertyFactory.CreateBlockListProperty("childBlocks", childBlockList) };
            parentContent.SetupGet(x => x.Properties).Returns(parentProperties);
            parentContent.Setup(x => x.GetProperty("childBlocks")).Returns(parentProperties[0]);
            parentContent.Setup(x => x.Value<OverridableBlockListModel>("childBlocks", null, null, It.IsAny<Fallback>(), null)).Returns(childBlockList);

            var parentBlockItem = new OverridableBlockListItem(
#nullable disable
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), parentContent.Object, null, null),
#nullable enable
                OverridableBlockListItem.NoopPublishedElementFactory
            );
            var parentBlockList = new OverridableBlockListModel(new[] { parentBlockItem }, null, OverridableBlockListItem.NoopPublishedElementFactory);


            // Act
            var result = parentBlockList.FindBlock(x => x.Content.GetProperty(EXAMPLE_TEXTBOX_PROPERTY_ALIAS) != null);

            // Assert
            Assert.IsInstanceOf<OverridableBlockListItem>(result);
        }
    }
}
