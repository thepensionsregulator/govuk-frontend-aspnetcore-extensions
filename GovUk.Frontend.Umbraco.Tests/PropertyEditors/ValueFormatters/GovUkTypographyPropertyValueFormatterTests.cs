using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [TestFixture]
    public class GovUkTypographyPropertyValueFormatterTests
    {
        [TestCase(Constants.PropertyEditors.Aliases.RichText, true)]
        [TestCase("someOtherPropertyEditor", false)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new GovUkTypographyPropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1, "someProperty", propertyEditorAlias, "someContentType", [], new RichTextConfiguration());

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
            const string EXPECTED = "<p class=\"govuk-body\">Example</p>";
            var formatter = new GovUkTypographyPropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.That(((HtmlEncodedString)resultOfString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
            Assert.That(((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
        }

        [Test]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new GovUkTypographyPropertyValueFormatter());
        }

        [TestCase("lower-alpha")]
        [TestCase("lower-greek")]
        [TestCase("lower-roman")]
        [TestCase("upper-alpha")]
        [TestCase("upper-roman")]
        public void Permitted_style_attribute_is_converted_to_class_from_ordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnOrderedLists(
                new GovUkTypographyPropertyValueFormatter(), listStyleType);
        }

        [Test]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new GovUkTypographyPropertyValueFormatter());
        }

        [TestCase("circle")]
        [TestCase("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new GovUkTypographyPropertyValueFormatter(), listStyleType);
        }

        [Test]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new GovUkTypographyPropertyValueFormatter());
        }

        [TestCase("text-align: center", "govuk-!-text-align-centre")]
        [TestCase("text-align: right", "govuk-!-text-align-right")]
        [TestCase("padding-left: 40px", "govuk-!-padding-left-7")]
        [TestCase("padding-left: 80px", "govuk-!-padding-left-14")]
        [TestCase("padding-left: 120px", "govuk-!-padding-left-21")]
        [TestCase("padding-left: 160px", "govuk-!-padding-left-28")]
        [TestCase("padding-left: 200px", "govuk-!-padding-left-35")]
        public void Permitted_style_attribute_is_converted_to_class_on_paragraphs(string styleAttribute, string expectedClass)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnParagraphs(
                new GovUkTypographyPropertyValueFormatter(), styleAttribute, expectedClass);
        }

        [Test]
        public void Style_attribute_is_removed_from_other_elements()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOtherElements(
                new GovUkTypographyPropertyValueFormatter());
        }
    }
}
