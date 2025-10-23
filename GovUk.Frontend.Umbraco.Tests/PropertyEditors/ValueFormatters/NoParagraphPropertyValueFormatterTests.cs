using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{    public class NoParagraphPropertyValueFormatterTests
    {
        [Theory]
        [InlineData(Constants.PropertyEditors.Aliases.TinyMce, false)]
        [InlineData(PropertyEditorAliases.GovUkInlineRichText, true)]
        [InlineData(PropertyEditorAliases.GovUkInlineInverseRichText, false)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphPropertyValueFormatter();
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
            var formatter = new NoParagraphPropertyValueFormatter();

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
                new NoParagraphPropertyValueFormatter());
        }

        [Fact]
        public void Single_wrapping_paragraph_with_class_is_left_alone()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithClassIsLeftAlone(
                new NoParagraphPropertyValueFormatter());
        }

        [Fact]
        public void Multiple_wrapping_paragraphs_are_left_alone()
        {
            TinyMCEValueFormattersTestHelper.MultipleWrappingParagraphsAreLeftAlone(
                 new NoParagraphPropertyValueFormatter());
        }

        [Fact]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new NoParagraphPropertyValueFormatter());
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
                new NoParagraphPropertyValueFormatter(), listStyleType);
        }

        [Fact]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new NoParagraphPropertyValueFormatter());
        }

        [Theory]
        [InlineData("circle")]
        [InlineData("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new NoParagraphPropertyValueFormatter(), listStyleType);
        }


        [Fact]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new NoParagraphPropertyValueFormatter());
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
                new NoParagraphPropertyValueFormatter(), styleAttribute, expectedClass);
        }
    }
}
