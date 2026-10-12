using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors.ValueConverters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;

namespace ThePensionsRegulator.Umbraco.Core.Tests.PropertyEditors.ValueConverters
{
    public class RichTextEditorPropertyValueConverterTests
    {
        [Fact]
        public void Applies_PropertyValueFormatters()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var propertyType = UmbracoPropertyFactory.CreateRichTextProperty("myAlias", "contentTypeAlias", new HtmlEncodedString(string.Empty)).PropertyType;

            const string INITIAL_VALUE = "<p>Some html</p>";
            const string EXPECTED_VALUE = "<p>Expected</p>";

            var formatter = new Mock<IPropertyValueFormatter>();
            formatter.Setup(x => x.IsFormatter(propertyType)).Returns(true);
            formatter.Setup(x => x.FormatValue(It.Is<HtmlEncodedString>(x => x.ToHtmlString() == INITIAL_VALUE))).Returns<HtmlEncodedString>(x => new HtmlEncodedString(EXPECTED_VALUE));

            var valueConverter = CreateValueConverter(testContext, [formatter.Object]);

            // Act
            var result = valueConverter.ConvertIntermediateToObject(
                UmbracoContentFactory.CreateContent<IPublishedElement>().Object,
                propertyType, PropertyCacheLevel.Element, new FakeRichTextIntermediateValue { Markup = INITIAL_VALUE }, false);

            // Assert
            Assert.Equal(EXPECTED_VALUE, ((IHtmlEncodedString?)result)?.ToHtmlString());
        }

        [Fact]
        public void IsConverter_Returns_True_For_RichTextEditor_Alias()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var valueConverter = CreateValueConverter(testContext);
            var propertyType = UmbracoPropertyFactory.CreateRichTextProperty("myAlias", "contentTypeAlias", new HtmlEncodedString(string.Empty)).PropertyType;

            // Act
            var result = valueConverter.IsConverter(propertyType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsConverter_Returns_False_For_Other_Editor_Alias()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var valueConverter = CreateValueConverter(testContext);
            var propertyType = UmbracoPropertyFactory.CreateTextboxProperty("myAlias", "contentTypeAlias", string.Empty).PropertyType;

            // Act
            var result = valueConverter.IsConverter(propertyType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetPropertyCacheLevel_Returns_None()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var valueConverter = CreateValueConverter(testContext);
            var propertyType = UmbracoPropertyFactory.CreateRichTextProperty("myAlias", "contentTypeAlias", new HtmlEncodedString(string.Empty)).PropertyType;

            // Act
            var result = valueConverter.GetPropertyCacheLevel(propertyType);

            // Assert
            Assert.Equal(PropertyCacheLevel.None, result);
        }

        private static RichTextEditorPropertyValueConverter CreateValueConverter(UmbracoTestContext testContext, IEnumerable<IPropertyValueFormatter>? propertyValueFormatters = null)
        {
            var urlProvider = testContext.PublishedUrlProvider.Object;

            var contentSettings = new Mock<IOptionsMonitor<ContentSettings>>();
            contentSettings.Setup(x => x.CurrentValue).Returns(new ContentSettings { ResolveUrlsFromTextString = false });

            var blockEditorVarianceHandler = new BlockEditorVarianceHandler(testContext.LanguageService.Object, testContext.ContentTypeService.Object);

            return new RichTextEditorPropertyValueConverter(
                new HtmlLocalLinkParser(urlProvider),
                new HtmlUrlParser(contentSettings.Object, Mock.Of<ILogger<HtmlUrlParser>>(), Mock.Of<IProfilingLogger>(), Mock.Of<IIOHelper>()),
                new HtmlImageSourceParser(urlProvider),
                propertyValueFormatters ?? new List<IPropertyValueFormatter>(),
                testContext.ApiRichTextElementParser.Object,
                testContext.ApiRichTextMarkupParser.Object,
                testContext.PartialViewBlockEngine.Object,
                new BlockEditorConverter(testContext.PublishedContentTypeCache.Object, testContext.CacheManager.Object, testContext.PublishedModelFactory.Object, testContext.VariationContextAccessor.Object, blockEditorVarianceHandler),
                testContext.JsonSerializer,
                testContext.ApiElementBuilder.Object,
                testContext.RichTextBlockPropertyValueConstructorCache.Object,
                Mock.Of<ILogger<RteBlockRenderingValueConverter>>(),
                blockEditorVarianceHandler,
                testContext.VariationContextAccessor.Object,
                Mock.Of<IOptionsMonitor<DeliveryApiSettings>>(),
                testContext.LanguageService.Object,
                testContext.PropertyRenderingContextAccessor.Object
                );
        }

        private class FakeRichTextIntermediateValue : IRichTextEditorIntermediateValue
        {
            public string Markup { get; set; } = string.Empty;

            public RichTextBlockModel? RichTextBlockModel { get; set; }
        }
    }
}
