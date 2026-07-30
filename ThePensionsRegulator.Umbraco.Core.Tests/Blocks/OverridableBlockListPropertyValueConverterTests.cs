using Microsoft.Extensions.Options;
using Moq;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using ThePensionsRegulator.Umbraco.Core.PropertyEditors;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace ThePensionsRegulator.Umbraco.Core.Tests.Blocks
{
    public class OverridableBlockListPropertyValueConverterTests
    {
        [Fact]
        public void Sets_PropertyValueFormatters()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var propertyType = UmbracoPropertyFactory.CreateBlockListProperty("myAlias", "contentTypeAlias", []).PropertyType;

            var initialValue = """{"contentData":[{"contentTypeKey":"90600a82-4925-4287-bb81-45247a91a514","key":"df3f71a6-de00-49a6-820a-ef247e6b675c","values":[{"alias":"linkText","culture":null,"editorAlias":null,"segment":null,"value":"Lorem ipsum dolor sit amet"},{"alias":"linkUrl","culture":null,"editorAlias":null,"segment":null,"value":"[{\u0022url\u0022:\u0022/success\u0022}]"}]},{"contentTypeKey":"90600a82-4925-4287-bb81-45247a91a514","key":"bccca89b-e21b-4beb-b2a7-710a5a5a5214","values":[{"alias":"linkText","culture":null,"editorAlias":null,"segment":null,"value":"Lorem ipsum dolor"},{"alias":"linkUrl","culture":null,"editorAlias":null,"segment":null,"value":"[{\u0022url\u0022:\u0022/success\u0022}]"}]}],"settingsData":[],"expose":[{"contentKey":"df3f71a6-de00-49a6-820a-ef247e6b675c","culture":null,"segment":null},{"contentKey":"bccca89b-e21b-4beb-b2a7-710a5a5a5214","culture":null,"segment":null}],"Layout":{"Umbraco.BlockList":[{"contentKey":"df3f71a6-de00-49a6-820a-ef247e6b675c","contentUdi":null,"settingsKey":null,"settingsUdi":null},{"contentKey":"bccca89b-e21b-4beb-b2a7-710a5a5a5214","contentUdi":null,"settingsKey":null,"settingsUdi":null}]}}""";

            var formatter = new Mock<IPropertyValueFormatter>();
            var valueConverter = CreateValueConverter(testContext, [formatter.Object]);

            // Act
            var result = valueConverter.ConvertIntermediateToObject(
                UmbracoContentFactory.CreateContent<IPublishedElement>().Object,
                propertyType, PropertyCacheLevel.Element, initialValue, false);

            // Assert
            Assert.Equal(formatter.Object, ((OverridableBlockListModel?)result)?.PropertyValueFormatters?.SingleOrDefault());
        }


        [Fact]
        public void GetPropertyCacheLevel_Returns_None()
        {
            // Arrange
            using var testContext = new UmbracoTestContext();
            var valueConverter = CreateValueConverter(testContext);
            var propertyType = UmbracoPropertyFactory.CreateBlockListProperty("myAlias", "contentTypeAlias", []).PropertyType;

            // Act
            var result = valueConverter.GetPropertyCacheLevel(propertyType);

            // Assert
            Assert.Equal(PropertyCacheLevel.None, result);
        }

        private static OverridableBlockListPropertyValueConverter CreateValueConverter(UmbracoTestContext testContext, IEnumerable<IPropertyValueFormatter>? propertyValueFormatters = null)
        {
            var contentSettings = new Mock<IOptionsMonitor<ContentSettings>>();
            contentSettings.Setup(x => x.CurrentValue).Returns(new ContentSettings { ResolveUrlsFromTextString = false });

            var blockEditorVarianceHandler = new BlockEditorVarianceHandler(testContext.LanguageService.Object, testContext.ContentTypeService.Object);

            return new OverridableBlockListPropertyValueConverter(
                testContext.ProfilingLogger.Object,
                new BlockEditorConverter(testContext.PublishedContentTypeCache.Object, testContext.CacheManager.Object, testContext.PublishedModelFactory.Object, testContext.VariationContextAccessor.Object, blockEditorVarianceHandler),
                testContext.ContentTypeService.Object,
                testContext.ApiElementBuilder.Object,
                testContext.JsonSerializer,
                testContext.BlockListPropertyValueConstructorCache.Object,
                testContext.VariationContextAccessor.Object,
                blockEditorVarianceHandler,
                testContext.PublishedValueFallback.Object,
                testContext.LanguageService.Object,
                testContext.PropertyRenderingContextAccessor.Object,
                propertyValueFormatters ?? new List<IPropertyValueFormatter>()
                );
        }
    }
}
