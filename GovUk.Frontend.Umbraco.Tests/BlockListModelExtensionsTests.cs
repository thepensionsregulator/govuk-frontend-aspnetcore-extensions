using GovUk.Frontend.Umbraco.BlockLists;
using NUnit.Framework;
using System.Linq;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class BlockListModelExtensionsTests
    {
        [Test]
        public void Block_is_matched_by_model_property()
        {
            var blockList = UmbracoBlockListFactory.CreateBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, "Field1")
                    .Object
                    )
                );

            // Act
            var result = BlockListModelExtensions.FindBlockByBoundProperty(blockList, "Field1");

            // Assert
            Assert.AreEqual(blockList.First(), result);
        }
    }
}
