using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Umbraco.Tests
{
    [Collection("UmbracoTests")]
    public class OverridablePublishedElementTests : IClassFixture<OverridablePublishedElementTests.Fixture>
    {
        private const string PROPERTY_ALIAS = "property";
        private const string ELEMENT_TYPE_ALIAS = "elementType";

        public class Fixture : IDisposable
        {
            private readonly UmbracoTestContext _context = new();

            public Fixture()
            {
                _context.SetupContentType(ELEMENT_TYPE_ALIAS);
            }

            public void Dispose() => _context.Dispose();
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
