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
        [InlineData(TprElementTypeAliases.Header, "someOtherProperty", false)]
        [InlineData("someOtherElementType", TprPropertyAliases.HeaderContent, false)]
        [InlineData(TprElementTypeAliases.Header, TprPropertyAliases.HeaderContent, true)]
        [InlineData(TprElementTypeAliases.ContextBarContext1, TprPropertyAliases.HeaderContext1, true)]
        [InlineData(TprElementTypeAliases.ContextBarContext2, TprPropertyAliases.HeaderContext2, true)]
        [InlineData(TprElementTypeAliases.ContextBarContext3, TprPropertyAliases.HeaderContext3, true)]
        [InlineData(TprElementTypeAliases.Footer, TprPropertyAliases.FooterContent, true)]
        public void Applies_only_to_correct_rich_text_properties(string compositionAlias, string propertyAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphsPropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1,
                propertyAlias,
                Constants.PropertyEditors.Aliases.RichText,
                "someContentType",
                [compositionAlias],
                new RichTextConfiguration());

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
