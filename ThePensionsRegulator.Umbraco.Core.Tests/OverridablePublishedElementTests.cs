using Moq;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Umbraco.Core.Tests
{
    public class OverridablePublishedElementTests : IDisposable
    {
        private const string PROPERTY_ALIAS = "property";
        private const string ELEMENT_TYPE_ALIAS = "elementType";
        private readonly UmbracoTestContext _testContext = new UmbracoTestContext().SetupContentType(ELEMENT_TYPE_ALIAS);
        private readonly OverridablePublishedElementValueStore _valueStore = new();

        public void Dispose() => _testContext.Dispose();

        [Fact]
        public void OverrideValue_works_for_HtmlEncodedString()
        {
            // Arrange
            var textBefore = new HtmlEncodedString("<p>This example has a {{token}} to update.</p>");
            var textAfter = new HtmlEncodedString("<p>This example has a thing to update.</p>");

            var content = new OverridablePublishedElement(UmbracoContentFactory.CreateContent<IPublishedElement>(ELEMENT_TYPE_ALIAS)
                .SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, textBefore)
                .Object, () => _valueStore);

            // Act
            content.OverrideValue(PROPERTY_ALIAS, textAfter);

            // Assert
            var updatedValue = content.Value<IHtmlEncodedString>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS);
            Assert.Equal(textAfter.ToString(), updatedValue?.ToString());
        }

        [Fact]
        public void OverrideValue_works_for_string()
        {
            // Arrange
            var textBefore = "<p>This example has a {{token}} to update.</p>";
            var textAfter = "<p>This example has a thing to update.</p>";

            var content = new OverridablePublishedElement(UmbracoContentFactory.CreateContent<IPublishedElement>(ELEMENT_TYPE_ALIAS)
                 .SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, textBefore)
                 .Object, () => _valueStore);

            // Act
            content.OverrideValue(PROPERTY_ALIAS, textAfter);

            // Assert
            var updatedValue = content.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS);
            Assert.Equal(textAfter, updatedValue);
        }

        [Fact]
        public void OverrideValue_is_isolated_between_value_stores_when_wrapper_is_reused()
        {
            // Arrange
            var originalValue = "<p>This example has a {{token}} to update.</p>";
            var firstRequestValue = "<p>This example has a first request override.</p>";
            var secondRequestValue = "<p>This example has a second request override.</p>";
            var firstStore = new OverridablePublishedElementValueStore();
            var secondStore = new OverridablePublishedElementValueStore();
            IOverridablePublishedElementValueStore currentStore = firstStore;

            var content = new OverridablePublishedElement(
                UmbracoContentFactory.CreateContent<IPublishedElement>(ELEMENT_TYPE_ALIAS)
                    .SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, originalValue)
                    .Object,
                () => currentStore);

            // Act
            content.OverrideValue(PROPERTY_ALIAS, firstRequestValue);
            currentStore = secondStore;
            var secondRequestBeforeOverride = content.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS);
            content.OverrideValue(PROPERTY_ALIAS, secondRequestValue);
            currentStore = firstStore;
            var firstRequestAfterSecondOverride = content.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS);
            currentStore = secondStore;
            var secondRequestAfterOverride = content.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS);

            // Assert
            Assert.Equal(originalValue, secondRequestBeforeOverride);
            Assert.Equal(firstRequestValue, firstRequestAfterSecondOverride);
            Assert.Equal(secondRequestValue, secondRequestAfterOverride);
        }

        [Fact]
        public void OverrideValue_is_shared_by_wrappers_for_the_same_element_key_within_one_store()
        {
            // Arrange
            var originalValue = "<p>This example has a {{token}} to update.</p>";
            var overrideValue = "<p>This example has a shared override.</p>";
            var publishedElement = UmbracoContentFactory.CreateContent<IPublishedElement>(ELEMENT_TYPE_ALIAS)
                .SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, originalValue)
                .Object;
            var firstWrapper = new OverridablePublishedElement(publishedElement, () => _valueStore);
            var secondWrapper = new OverridablePublishedElement(publishedElement, () => _valueStore);

            // Act
            firstWrapper.OverrideValue(PROPERTY_ALIAS, overrideValue);

            // Assert
            Assert.Equal(overrideValue, secondWrapper.Value<string>(_testContext.PublishedValueFallback.Object, PROPERTY_ALIAS));
        }
    }
}
