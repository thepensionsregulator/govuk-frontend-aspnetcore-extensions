using GovUk.Frontend.Umbraco.Models;
using Moq;
using NUnit.Framework;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class OverridableBlockListModelTests : UmbracoBaseTest
    {
        [Test]
        public void BlockListModels_are_converted_to_OverridableBlockListModels_including_nested_block_lists()
        {
            var blockListPropertyType = CreatePropertyType(1, Constants.PropertyEditors.Aliases.BlockList, new BlockListConfiguration());

            var grandChildBlockListContentProperties = Array.Empty<IPublishedProperty>();
            var grandChildBlockList = CreateBlockListModel(grandChildBlockListContentProperties, Array.Empty<IPublishedProperty>());
            var childBlockListContentProperties = new[] { CreateProperty("grandchildBlocks", blockListPropertyType, grandChildBlockList) };
            var childBlockList = CreateBlockListModel(childBlockListContentProperties, Array.Empty<IPublishedProperty>());
            var parentBlockListContentProperties = new[] { CreateProperty("childBlocks", blockListPropertyType, childBlockList) };
            var parentBlockList = CreateBlockListModel(parentBlockListContentProperties, Array.Empty<IPublishedProperty>());

            OverridableBlockListModel? convertedChildBlockList = null, convertedGrandChildBlockList = null;

            var parentBlockListContent = new Mock<IOverridablePublishedElement>();
            parentBlockListContent.Setup(x => x.OverrideValue("childBlocks", It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedChildBlockList = value as OverridableBlockListModel;
            });
            parentBlockListContent.Setup(x => x.Properties).Returns(parentBlockListContentProperties);
            parentBlockListContent.Setup(x => x.Value<BlockListModel>("childBlocks", null, null, default, default)).Returns(childBlockList);

            var childBlockListContent = new Mock<IOverridablePublishedElement>();
            childBlockListContent.Setup(x => x.OverrideValue("grandchildBlocks", It.IsAny<object>())).Callback<string, object>((alias, value) =>
            {
                convertedGrandChildBlockList = value as OverridableBlockListModel;
            });
            childBlockListContent.Setup(x => x.Properties).Returns(childBlockListContentProperties);
            childBlockListContent.Setup(x => x.Value<BlockListModel>("grandchildBlocks", null, null, default, default)).Returns(grandChildBlockList);

            var grandChildBlockListContent = new Mock<IOverridablePublishedElement>();
            grandChildBlockListContent.Setup(x => x.Properties).Returns(grandChildBlockListContentProperties);


            var factoryCalls = 0;
            Func<IPublishedElement, IOverridablePublishedElement> factory = x =>
            {
                factoryCalls++;
                switch (factoryCalls)

                {
                    case 1:
                        return parentBlockListContent.Object;
                    case 3:
                        return childBlockListContent.Object;
                    case 5:
                        return grandChildBlockListContent.Object;
                    default:
                        return null;
                }
            };

            new OverridableBlockListModel(parentBlockList, null, factory);
            Assert.NotNull(convertedChildBlockList);
            Assert.NotNull(convertedGrandChildBlockList);
        }
    }
}
