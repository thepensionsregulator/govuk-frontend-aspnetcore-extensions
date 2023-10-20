using GovUk.Frontend.Umbraco.Blocks;
using NUnit.Framework;
using System.Linq;
using ThePensionsRegulator.Umbraco.Testing;

namespace GovUk.Frontend.Umbraco.Tests.Blocks
{
    public class OverridableBlockModelExtensionsTests
    {
        [TestCase("Field1", true)]
        [TestCase("Field2", false)]
        public void Block_is_matched_by_model_property(string propertyName, bool expected)
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateOverridableBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.ModelProperty, "Field1")
                    .Object
                    )
                );

            // Act
            var result = blockList.FindBlockByBoundProperty(propertyName);

            // Assert
            if (expected)
            {
                Assert.AreEqual(blockList.First(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [TestCase("example-b", true)]
        [TestCase("example-c", false)]
        public void Block_is_matched_by_class(string className, bool expected)
        {
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClasses, "example-a example-b")
                    .Object
                    )
                );

            // Act
            var result = blockList.FindBlockByClass(className);

            // Assert
            if (expected)
            {
                Assert.AreEqual(blockList.First(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [TestCase("example-b", true)]
        [TestCase("example-c", true)]
        [TestCase("example-f", false)]
        public void Block_is_matched_by_class_from_multiple_block_lists(string className, bool expected)
        {
            var blockList1 = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClasses, "example-a example-b")
                    .Object
                    )
                );
            var blockList2 = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClasses, "example-c example-d")
                    .Object
                    )
                );

            var blockLists = new[] { blockList1, blockList2 };

            // Act
            var result = blockLists.FindBlockByClass(className);

            // Assert
            if (expected)
            {
                Assert.NotNull(result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [TestCase("example-b", 2)]
        [TestCase("example-f", 0)]
        public void Multiple_blocks_are_matched_by_class_from_multiple_block_lists(string className, int expected)
        {
            var blockList1 = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClasses, "example-a example-b")
                    .Object
                    )
                );
            var blockList2 = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                UmbracoBlockListFactory.CreateBlock(
                    UmbracoBlockListFactory.CreateContentOrSettings().Object,
                    UmbracoBlockListFactory.CreateContentOrSettings()
                    .SetupUmbracoTextboxPropertyValue(PropertyAliases.CssClasses, "example-b example-d")
                    .Object
                    )
                );

            var blockLists = new[] { blockList1, blockList2 };

            // Act
            var result = blockLists.FindBlocksByClass(className);

            // Assert
            Assert.That(result.Count(), Is.EqualTo(expected));
        }
    }
}
