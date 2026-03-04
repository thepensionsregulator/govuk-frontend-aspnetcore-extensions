using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Umbraco.Core.Tests
{
    public class OverridablePublishedElementTests
    {
        private const string PROPERTY_ALIAS = "property";
        private const string ELEMENT_TYPE_ALIAS = "elementType";

        public OverridablePublishedElementTests()
        {
            var testContext = new UmbracoTestContext()
                .SetupContentType(ELEMENT_TYPE_ALIAS);
        }

        [Fact]
        public void OverrideValue_works_for_HtmlEncodedString()
        {
            // Arrange
            var textBefore = new HtmlEncodedString("<p>This example has a {{token}} to update.</p>");
            var textAfter = new HtmlEncodedString("<p>This example has a thing to update.</p>");

            var content = new OverridablePublishedElement(UmbracoContentFactory.CreateContent<IPublishedElement>(ELEMENT_TYPE_ALIAS)
                .SetupUmbracoRichTextPropertyValue(PROPERTY_ALIAS, textBefore)
                .Object);

            // Act
            content.OverrideValue(PROPERTY_ALIAS, textAfter);

            // Assert
            var updatedValue = content.Value<IHtmlEncodedString>(PROPERTY_ALIAS);
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
                 .Object);

            // Act
            content.OverrideValue(PROPERTY_ALIAS, textAfter);

            // Assert
            var updatedValue = content.Value<string>(PROPERTY_ALIAS);
            Assert.Equal(textAfter, updatedValue);
        }
    }
}
