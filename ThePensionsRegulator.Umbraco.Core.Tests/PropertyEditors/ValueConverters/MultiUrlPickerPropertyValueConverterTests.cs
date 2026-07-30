using Moq;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors.ValueConverters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace ThePensionsRegulator.Umbraco.Core.Tests.PropertyEditors.ValueConverters
{
    public class MultiUrlPickerPropertyValueConverterTests
    {
        [Fact]
        public void Applies_PropertyValueFormatters()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var propertyType = UmbracoPropertyFactory.CreateMultiUrlPickerProperty("myAlias", "contentTypeAlias", new Link()).PropertyType;

            var initialValue = """[{"name":"Home","target":null,"unique":null,"type":null,"udi":"umb://document/5e682cbeb867491a955f3382446d5663","url":null,"queryString":null}]""";
            var expectedValue = new List<Link> { new Link { Name = "Expected" } };

            var formatter = new Mock<IPropertyValueFormatter>();
            formatter.Setup(x => x.IsFormatter(propertyType)).Returns(true);
            formatter.Setup(x => x.FormatValue(It.IsAny<List<Link>>())).Returns<List<Link>>(x => expectedValue);

            var valueConverter = CreateValueConverter(testContext, [formatter.Object]);

            // Act
            var result = valueConverter.ConvertIntermediateToObject(
                UmbracoContentFactory.CreateContent<IPublishedElement>().Object,
                propertyType, PropertyCacheLevel.Element, initialValue, false);

            // Assert
            Assert.Equal(expectedValue[0].Name, ((List<Link>?)result)?[0].Name);
        }

        [Fact]
        public void GetPropertyCacheLevel_Returns_None()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var valueConverter = CreateValueConverter(testContext);
            var propertyType = UmbracoPropertyFactory.CreateMultiUrlPickerProperty("myAlias", "contentTypeAlias", new Link()).PropertyType;

            // Act
            var result = valueConverter.GetPropertyCacheLevel(propertyType);

            // Assert
            Assert.Equal(PropertyCacheLevel.None, result);
        }

        private static MultiUrlPickerPropertyValueConverter CreateValueConverter(UmbracoTestContext testContext, IEnumerable<IPropertyValueFormatter>? propertyValueFormatters = null)
        {
            return new MultiUrlPickerPropertyValueConverter(
                testContext.ProfilingLogger.Object,
                testContext.JsonSerializer,
                testContext.PublishedUrlProvider.Object,
                propertyValueFormatters ?? new List<IPropertyValueFormatter>(),
                testContext.ApiContentNameProvider.Object,
                testContext.ApiMediaUrlProvider.Object,
                testContext.ApiContentRouteBuilder.Object,
                testContext.PublishedContentCache.Object,
                testContext.PublishedMediaCache.Object
                );
        }
    }
}
