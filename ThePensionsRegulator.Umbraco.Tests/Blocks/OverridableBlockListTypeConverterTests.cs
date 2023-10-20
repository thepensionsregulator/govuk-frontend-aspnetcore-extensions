using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockListTypeConverterTests
    {
        [Test]
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
            Assert.That(converted, Is.Not.Null);
            Assert.That(converted.Count, Is.EqualTo(blockList.Count()));
        }
    }
}
