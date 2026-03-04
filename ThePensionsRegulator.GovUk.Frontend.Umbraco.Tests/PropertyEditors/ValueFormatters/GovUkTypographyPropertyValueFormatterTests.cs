using ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    public class GovUkTypographyPropertyValueFormatterTests
    {
        [Theory]
        [InlineData(Constants.PropertyEditors.Aliases.RichText, true)]
        [InlineData("someOtherPropertyEditor", false)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new GovUkTypographyPropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1, "someProperty", propertyEditorAlias, "someContentType", [], new RichTextConfiguration());

            // Act
            var result = formatter.IsFormatter(propertyType);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Accepts_string_or_HtmlEncodedString_as_input()
        {
            // Arrange
            const string INPUT = "<p>Example</p>";
            const string EXPECTED = "<p class=\"govuk-body\">Example</p>";
            var formatter = new GovUkTypographyPropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfString)?.ToHtmlString());
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new GovUkTypographyPropertyValueFormatter());
        }

        [Theory]
        [InlineData("lower-alpha")]
        [InlineData("lower-greek")]
        [InlineData("lower-roman")]
        [InlineData("upper-alpha")]
        [InlineData("upper-roman")]
        public void Permitted_style_attribute_is_converted_to_class_from_ordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnOrderedLists(
                new GovUkTypographyPropertyValueFormatter(), listStyleType);
        }

        [Fact]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new GovUkTypographyPropertyValueFormatter());
        }

        [Theory]
        [InlineData("circle")]
        [InlineData("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new GovUkTypographyPropertyValueFormatter(), listStyleType);
        }

        [Fact]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new GovUkTypographyPropertyValueFormatter());
        }

        [Theory]
        [InlineData("text-align: center", "govuk-!-text-align-centre")]
        [InlineData("text-align: right", "govuk-!-text-align-right")]
        [InlineData("padding-left: 40px", "govuk-!-padding-left-7")]
        [InlineData("padding-left: 80px", "govuk-!-padding-left-14")]
        [InlineData("padding-left: 120px", "govuk-!-padding-left-21")]
        [InlineData("padding-left: 160px", "govuk-!-padding-left-28")]
        [InlineData("padding-left: 200px", "govuk-!-padding-left-35")]
        public void Permitted_style_attribute_is_converted_to_class_on_paragraphs(string styleAttribute, string expectedClass)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnParagraphs(
                new GovUkTypographyPropertyValueFormatter(), styleAttribute, expectedClass);
        }

        [Fact]
        public void Style_attribute_is_removed_from_other_elements()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOtherElements(
                new GovUkTypographyPropertyValueFormatter());
        }
    }
}
