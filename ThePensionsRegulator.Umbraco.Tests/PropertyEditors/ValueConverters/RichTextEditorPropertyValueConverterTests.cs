using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using ThePensionsRegulator.Umbraco.PropertyEditors.ValueConverters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Blocks;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Macros;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Core.Templates;

namespace ThePensionsRegulator.Umbraco.Tests.PropertyEditors.ValueConverters
{
    [TestFixture]
    public class RichTextEditorPropertyValueConverterTests
    {
        [Test]
        public void Applies_PropertyValueFormatters()
        {
            // Arrange
            var testContext = new UmbracoTestContext();
            var propertyType = UmbracoPropertyFactory.CreateRichTextProperty("myAlias", string.Empty).PropertyType;
            var urlProvider = Mock.Of<IPublishedUrlProvider>();

            const string INITIAL_VALUE = "<p>Some html</p>";
            const string EXPECTED_VALUE = "<p>Expected</p>";

            var formatter = new Mock<IPropertyValueFormatter>();
            formatter.Setup(x => x.IsFormatter(propertyType)).Returns(true);
            formatter.Setup(x => x.FormatValue(It.Is<HtmlEncodedString>(x => x.ToHtmlString() == INITIAL_VALUE))).Returns<HtmlEncodedString>(x => new HtmlEncodedString(EXPECTED_VALUE));

            var contentSettings = new Mock<IOptionsMonitor<ContentSettings>>();
            contentSettings.Setup(x => x.CurrentValue).Returns(new ContentSettings { ResolveUrlsFromTextString = false });

            var valueConverter = new RichTextEditorPropertyValueConverter(
                testContext.UmbracoContextAccessor.Object,
                Mock.Of<IMacroRenderer>(),
                new HtmlLocalLinkParser(testContext.UmbracoContextAccessor.Object, urlProvider),
                new HtmlUrlParser(contentSettings.Object, Mock.Of<ILogger<HtmlUrlParser>>(), Mock.Of<IProfilingLogger>(), Mock.Of<IIOHelper>()),
                new HtmlImageSourceParser(urlProvider),
                new List<IPropertyValueFormatter> { formatter.Object },
                Mock.Of<IApiRichTextElementParser>(),
                Mock.Of<IApiRichTextMarkupParser>(),
                Mock.Of<IPartialViewBlockEngine>(),
                new BlockEditorConverter(testContext.PublishedSnapshotAccessor.Object, testContext.PublishedModelFactory.Object),
                Mock.Of<IJsonSerializer>(),
                Mock.Of<IApiElementBuilder>(),
                Mock.Of<RichTextBlockPropertyValueConstructorCache>(),
                Mock.Of<ILogger<RteMacroRenderingValueConverter>>(),
                Mock.Of<IOptionsMonitor<DeliveryApiSettings>>()
                );

            // Act
            var result = valueConverter.ConvertIntermediateToObject(
                UmbracoContentFactory.CreateContent<IPublishedElement>().Object,
                propertyType, PropertyCacheLevel.Snapshot, new FakeRichTextIntermediateValue { Markup = INITIAL_VALUE }, false);

            // Assert
            Assert.That(((IHtmlEncodedString?)result)?.ToHtmlString(), Is.EqualTo(EXPECTED_VALUE));
        }

        private class FakeRichTextIntermediateValue : IRichTextEditorIntermediateValue
        {
            public string Markup { get; set; } = string.Empty;

            public RichTextBlockModel? RichTextBlockModel { get; set; }
        }
    }
}
