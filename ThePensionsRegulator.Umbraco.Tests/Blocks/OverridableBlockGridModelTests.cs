using Moq;
using System.ComponentModel;
using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockGridModelTests
    {
        private const string DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS = "docTypeChildBlocks";
        private const string PROPERTY_ALIAS_CHILD_BLOCKS = "childBlocks";

        public OverridableBlockGridModelTests()
        {
            _ = new UmbracoTestContext(); // Sets up Umbraco dependency injection
        }

        private static (OverridableBlockGridModel ParentBlockGrid, OverridableBlockGridModel ChildBlockGrid, OverridableBlockGridModel GrandChildBlockGrid) CreateThreeNestedOverridableBlockGrids()
        {
            var grandChildBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(Array.Empty<BlockGridItem>());

            var childContent = UmbracoBlockGridFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, grandChildBlockGrid)
                    .Object;

            var childBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(childContent)
                );

            var parentContent = UmbracoBlockGridFactory.CreateContentOrSettings(DOCUMENT_TYPE_ALIAS_CHILD_BLOCKS)
                    .SetupUmbracoBlockGridPropertyValue(PROPERTY_ALIAS_CHILD_BLOCKS, childBlockGrid)
                    .Object;

            var parentBlockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                UmbracoBlockGridFactory.CreateOverridableBlock(parentContent)
                );

            return (parentBlockGrid, childBlockGrid, grandChildBlockGrid);
        }

        [Fact]
        public void Simple_placeholder_test()
        {
            // This is a placeholder test that we'll use to ensure compilation works
            Assert.True(true);
        }
    }
}
