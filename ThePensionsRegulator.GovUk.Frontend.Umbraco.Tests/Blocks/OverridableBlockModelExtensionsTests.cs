using ThePensionsRegulator.GovUk.Frontend.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Blocks
{
    public class OverridableBlockModelExtensionsTests
    {
        [Theory]
        [InlineData("Field1", true)]
        [InlineData("Field2", false)]
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
                Assert.Equal(blockList.First(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("example-b", true)]
        [InlineData("example-c", false)]
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
                Assert.Equal(blockList.First(), result);
            }
            else
            {
                Assert.Null(result);
            }
        }

        [Theory]
        [InlineData("example-b", true)]
        [InlineData("example-c", true)]
        [InlineData("example-f", false)]
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

        [Theory]
        [InlineData("example-b", 2)]
        [InlineData("example-f", 0)]
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
            Assert.Equal(expected, result.Count());
        }
    }
}
