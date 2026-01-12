using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    public class NoParagraphsPropertyValueFormatterTests
    {
        [Theory]
        [InlineData(Constants.PropertyEditors.Aliases.RichText, false)]
        [InlineData(GovUk.Frontend.Umbraco.PropertyEditorAliases.GovUkInlineRichText, false)]
        [InlineData(GovUk.Frontend.Umbraco.PropertyEditorAliases.GovUkInlineInverseRichText, false)]
        [InlineData(TprPropertyEditorAliases.TprHeaderFooterRichText, true)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphsPropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1, propertyEditorAlias, new RichTextConfiguration());

            // Act
            var result = formatter.IsFormatter(propertyType);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Accepts_string_or_HtmlEncodedString_as_input()
        {
            // Arrange
            const string INPUT = "<p>Example</p><p>Example</p>";
            const string EXPECTED = "ExampleExample";
            var formatter = new NoParagraphsPropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfString)?.ToHtmlString());
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }
    }
}
