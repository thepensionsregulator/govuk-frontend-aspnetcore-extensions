using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{    public class NoParagraphInversePropertyValueFormatterTests
    {
        [Theory]
        [InlineData(Constants.PropertyEditors.Aliases.TinyMce, false)]
        [InlineData(PropertyEditorAliases.GovUkInlineRichText, false)]
        [InlineData(PropertyEditorAliases.GovUkInlineInverseRichText, true)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphInversePropertyValueFormatter();
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
            const string INPUT = "<p>Example</p>";
            const string EXPECTED = "Example";
            var formatter = new NoParagraphInversePropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfString)?.ToHtmlString());
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Single_wrapping_paragraph_with_no_class_is_removed()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithNoClassIsRemoved(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Fact]
        public void Single_wrapping_paragraph_with_class_is_left_alone()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithClassIsLeftAlone(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Fact]
        public void Multiple_wrapping_paragraphs_are_left_alone()
        {
            TinyMCEValueFormattersTestHelper.MultipleWrappingParagraphsAreLeftAlone(
                 new NoParagraphInversePropertyValueFormatter());
        }

        [Fact]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new NoParagraphInversePropertyValueFormatter());
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
                new NoParagraphInversePropertyValueFormatter(), listStyleType);
        }

        [Fact]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Theory]
        [InlineData("circle")]
        [InlineData("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new NoParagraphInversePropertyValueFormatter(), listStyleType);
        }

        [Fact]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Theory]
        [InlineData("text-align: center", "govuk-!-text-align-centre")]
        [InlineData("text-align: right", "govuk-!-text-align-right")]
        public void Permitted_style_attribute_is_converted_to_class_on_paragraphs(string styleAttribute, string expectedClass)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnParagraphs(
                new NoParagraphInversePropertyValueFormatter(), styleAttribute, expectedClass);
        }
    }
}
