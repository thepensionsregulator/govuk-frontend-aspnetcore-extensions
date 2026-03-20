using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    public class TprTablePropertyValueFormatterTests
    {
        [Fact]
        public void Applies_to_rich_text_properties_with_any_alias()
        {
            // Arrange
            var formatter = new TprTablePropertyValueFormatter();
            var property = UmbracoPropertyFactory.CreateRichTextProperty(
                Guid.NewGuid().ToString(),
                "someContentType",
                null);

            // Act
            var result = formatter.IsFormatter(property.PropertyType);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Does_not_apply_to_textbox_property()
        {
            // Arrange
            var formatter = new TprTablePropertyValueFormatter();
            var property = UmbracoPropertyFactory.CreateTextboxProperty(
                Guid.NewGuid().ToString(),
                "someContentType",
                "");

            // Act
            var result = formatter.IsFormatter(property.PropertyType);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Accepts_string_or_HtmlEncodedString_as_input()
        {
            // Arrange
            const string INPUT = "<table><tr><td>Example</td><td>Example</td></tr></table>";
            const string EXPECTED = $"<table class=\"{TprClassNames.Table}\"><tr><td>Example</td><td>Example</td></tr></table>";
            var formatter = new TprTablePropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfString)?.ToHtmlString());
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Does_not_affect_other_applied_classes()
        {
            // Arrange
            const string INPUT = "<table class=\"existing-class\"><tr><td>Example</td><td>Example</td></tr></table>";
            const string EXPECTED = $"<table class=\"existing-class {TprClassNames.Table}\"><tr><td>Example</td><td>Example</td></tr></table>";
            var formatter = new TprTablePropertyValueFormatter();

            // Act
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Does_not_duplicate_class()
        {
            // Arrange
            const string INPUT = $"<table class=\"{TprClassNames.Table}\"><tr><td>Example</td><td>Example</td></tr></table>";
            const string EXPECTED = $"<table class=\"{TprClassNames.Table}\"><tr><td>Example</td><td>Example</td></tr></table>";
            var formatter = new TprTablePropertyValueFormatter();

            // Act
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Does_not_apply_to_other_elements()
        {
            // Arrange
            const string INPUT = "<h2>Example</h2><p>Example</p><strong>Example</strong>";
            const string EXPECTED = INPUT;
            var formatter = new TprTablePropertyValueFormatter();

            // Act
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }
    }
}
