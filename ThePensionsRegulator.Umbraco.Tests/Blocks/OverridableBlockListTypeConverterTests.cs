using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockListTypeConverterTests
    {
        [Fact]
        public void Can_convert()
        {
            // Arrange
            var blockList = UmbracoBlockListFactory.CreateOverridableBlockListModel(
                                UmbracoBlockListFactory.CreateOverridableBlock(
                                    UmbracoBlockListFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            var converter = new OverridableBlockListTypeConverter();

            // Act
            BlockListModel? converted = null;
            if (converter.CanConvertTo(typeof(BlockListModel)))
            {
                converted = converter.ConvertTo(blockList, typeof(BlockListModel)) as BlockListModel;
            }

            // Assert
            Assert.NotNull(converted);
            Assert.Equal(converted.Count, blockList.Count());
        }
    }
}
