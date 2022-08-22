using GovUk.Frontend.Umbraco.Models;
using NUnit.Framework;
using System;
using System.Linq;
using Umbraco.Cms.Core;
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
    }
}
