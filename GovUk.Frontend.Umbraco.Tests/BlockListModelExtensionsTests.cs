using GovUk.Frontend.Umbraco.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class BlockListModelExtensionsTests : UmbracoBaseTest
    {
        [Test]
        public void Block_is_matched_in_root_block_list()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var blockListContentProperties = new[] {
                CreateProperty("MyTextProperty", textBoxPropertyType, "value")
            };
            var blockList = CreateBlockListModel(blockListContentProperties, Array.Empty<IPublishedProperty>());

            // Act
            var result = BlockListModelExtensions.FindBlock(blockList, x => x.Content.GetProperty("MyTextProperty") != null);

            // Assert
            Assert.AreEqual(blockList.First(), result);
        }

        [Test]
        public void Block_is_matched_in_descendant_block_list()
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var grandChildBlockListContentProperties = new[] {
                CreateProperty("MyTextProperty", textBoxPropertyType, "value")
            };
            var grandChildBlockList = CreateBlockListModel(grandChildBlockListContentProperties, Array.Empty<IPublishedProperty>());
            var childBlockListContentProperties = new[] { CreateProperty("grandchildBlocks", blockListPropertyType, grandChildBlockList) };
            var childBlockList = CreateBlockListModel(childBlockListContentProperties, Array.Empty<IPublishedProperty>());
            var parentBlockListContentProperties = new[] { CreateProperty("childBlocks", blockListPropertyType, childBlockList) };
            var parentBlockList = CreateBlockListModel(parentBlockListContentProperties, Array.Empty<IPublishedProperty>());

            // Act
            var result = BlockListModelExtensions.FindBlock(parentBlockList, x => x.Content.GetProperty("MyTextProperty") != null);

            // Assert
            Assert.AreEqual(grandChildBlockList.First(), result);
        }

        [Test]
        public void Block_is_matched_by_model_property()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var blockListSettingsProperties = new[] {
                CreateProperty(PropertyAliases.ModelProperty, textBoxPropertyType, "Field1")
            };
            var blockList = CreateBlockListModel(Array.Empty<IPublishedProperty>(), blockListSettingsProperties);

            // Act
            var result = BlockListModelExtensions.FindBlockByBoundProperty(blockList, "Field1");

            // Assert
            Assert.AreEqual(blockList.First(), result);
        }

        [Test]
        public void Block_is_matched_by_content_type_alias()
        {
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var contentType = new Mock<IPublishedContentType>();
            contentType.Setup(x => x.Alias).Returns("myAlias");
            var blockContent = new Mock<IPublishedElement>();
            blockContent.SetupGet(x => x.ContentType).Returns(contentType.Object);

            var blockList = new BlockListModel(new List<BlockListItem> {
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), blockContent.Object, null, null)
            });

            // Act
            var result = BlockListModelExtensions.FindBlockByContentTypeAlias(blockList, "myAlias");

            // Assert
            Assert.AreEqual(blockList.First(), result);
        }

        [Test]
        public void OverridableBlockListModel_returns_OverridableBlockListItem()
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());
            var textBoxPropertyType = CreatePropertyType(2, Constants.PropertyEditors.Aliases.TextBox, new TextboxConfiguration());

            var childContent = new Mock<IOverridablePublishedElement>();
            childContent.Setup(x => x.GetProperty("MyTextProperty")).Returns(CreateProperty("MyTextProperty", textBoxPropertyType, "value"));

            var childBlockItem = new OverridableBlockListItem(
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), childContent.Object, null, null),
                x => (IOverridablePublishedElement)x
            );

            var childBlockList = new OverridableBlockListModel(new[] { childBlockItem }, null, x => (IOverridablePublishedElement)x);


            var parentContent = new Mock<IOverridablePublishedElement>();
            var parentProperties = new[] { CreateProperty("childBlocks", blockListPropertyType, childBlockList) };
            parentContent.SetupGet(x => x.Properties).Returns(parentProperties);
            parentContent.Setup(x => x.GetProperty("childBlocks")).Returns(parentProperties[0]);
            parentContent.Setup(x => x.Value<OverridableBlockListModel>("childBlocks", null, null, It.IsAny<Fallback>(), null)).Returns(childBlockList);

            var parentBlockItem = new OverridableBlockListItem(
                new BlockListItem(Udi.Create(Constants.UdiEntityType.Element, Guid.NewGuid()), parentContent.Object, null, null),
                x => (IOverridablePublishedElement)x
            );
            var parentBlockList = new OverridableBlockListModel(new[] { parentBlockItem }, null, x => (IOverridablePublishedElement)x);


            // Act
            var result = BlockListModelExtensions.FindBlock(parentBlockList, x => x.Content.GetProperty("MyTextProperty") != null);

            // Assert
            Assert.IsInstanceOf<OverridableBlockListItem>(result);
        }
    }
}
