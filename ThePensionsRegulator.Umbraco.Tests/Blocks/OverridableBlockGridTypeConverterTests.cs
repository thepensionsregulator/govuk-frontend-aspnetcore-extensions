using ThePensionsRegulator.Umbraco.Blocks;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.Blocks;

namespace ThePensionsRegulator.Umbraco.Tests.Blocks
{
    public class OverridableBlockGridTypeConverterTests
    {
        [Test]
        public void Can_convert()
        {
            // Arrange
            var blockGrid = UmbracoBlockGridFactory.CreateOverridableBlockGridModel(
                                UmbracoBlockGridFactory.CreateOverridableBlock(
                                    UmbracoBlockGridFactory.CreateContentOrSettings("alias").Object
                                    )
                                );

            var converter = new OverridableBlockGridTypeConverter();

            // Act
            BlockGridModel? converted = null;
            if (converter.CanConvertTo(typeof(BlockGridModel)))
            {
                converted = converter.ConvertTo(blockGrid, typeof(BlockGridModel)) as BlockGridModel;
            }

            // Assert
            Assert.That(converted, Is.Not.Null);
            Assert.That(converted.Count, Is.EqualTo(blockGrid.Count()));
        }
    }
}
