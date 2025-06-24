using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [TestFixture]
    public class NoParagraphInversePropertyValueFormatterTests
    {
        [TestCase(Constants.PropertyEditors.Aliases.TinyMce, false)]
        [TestCase(PropertyEditorAliases.GovUkInlineRichText, false)]
        [TestCase(PropertyEditorAliases.GovUkInlineInverseRichText, true)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphInversePropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1, propertyEditorAlias, new RichTextConfiguration());

            // Act
            var result = formatter.IsFormatter(propertyType);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
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
            Assert.That(((HtmlEncodedString)resultOfString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
            Assert.That(((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
        }

        [Test]
        public void Single_wrapping_paragraph_with_no_class_is_removed()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithNoClassIsRemoved(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Test]
        public void Single_wrapping_paragraph_with_class_is_left_alone()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithClassIsLeftAlone(
                new NoParagraphInversePropertyValueFormatter());
        }

        [Test]
        public void Multiple_wrapping_paragraphs_are_left_alone()
        {
            TinyMCEValueFormattersTestHelper.MultipleWrappingParagraphsAreLeftAlone(
                 new NoParagraphInversePropertyValueFormatter());
        }

        [Test]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new NoParagraphInversePropertyValueFormatter());
        }

        [TestCase("lower-alpha")]
        [TestCase("lower-greek")]
        [TestCase("lower-roman")]
        [TestCase("upper-alpha")]
        [TestCase("upper-roman")]
        public void Permitted_style_attribute_is_converted_to_class_from_ordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnOrderedLists(
                new NoParagraphInversePropertyValueFormatter(), listStyleType);
        }

        [Test]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new NoParagraphInversePropertyValueFormatter());
        }

        [TestCase("circle")]
        [TestCase("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new NoParagraphInversePropertyValueFormatter(), listStyleType);
        }

        [Test]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new NoParagraphInversePropertyValueFormatter());
        }

        [TestCase("center")]
        [TestCase("right")]
        public void Permitted_style_attribute_is_converted_to_class_on_paragraphs(string alignmentStyle)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnParagraphs(
                new NoParagraphInversePropertyValueFormatter(), alignmentStyle);
        }
    }
}
