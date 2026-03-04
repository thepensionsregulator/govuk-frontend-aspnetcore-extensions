using NUnit.Framework;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [TestFixture]
    public class NoParagraphPropertyValueFormatterTests
    {
        [TestCase("someOtherElementType", true, PropertyAliases.Hint, false)]
        [TestCase(ElementTypeAliases.Hint, true, "someOtherProperty", false)]
        [TestCase(ElementTypeAliases.Hint, true, PropertyAliases.Hint, true)]
        [TestCase(ElementTypeAliases.Task, false, PropertyAliases.Hint, true)]
        [TestCase(ElementTypeAliases.Details, false, PropertyAliases.DetailsText, true)]
        [TestCase(ElementTypeAliases.InsetText, false, PropertyAliases.InsetText, true)]
        [TestCase(ElementTypeAliases.WarningText, false, PropertyAliases.WarningText, true)]
        [TestCase(ElementTypeAliases.PhaseBanner, true, PropertyAliases.PhaseBannerText, true)]
        [TestCase(ElementTypeAliases.NotificationBanner, false, PropertyAliases.NotificationBannerHeading, true)]
        [TestCase(ElementTypeAliases.SummaryListItem, false, PropertyAliases.SummaryListItemValue, true)]
        public void Applies_only_to_correct_rich_text_properties(string contentTypeAlias, bool isComposition, string propertyAlias, bool expected)
        {
            // Arrange
            var formatter = new NoParagraphPropertyValueFormatter();
            var propertyType = UmbracoPropertyFactory.CreatePropertyType(1,
                propertyAlias,
                Constants.PropertyEditors.Aliases.RichText,
                isComposition ? "someContentType" : contentTypeAlias,
                isComposition ? [contentTypeAlias] : [],
                new RichTextConfiguration());

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
            var formatter = new NoParagraphPropertyValueFormatter();

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
                new NoParagraphPropertyValueFormatter());
        }

        [Test]
        public void Single_wrapping_paragraph_with_class_is_left_alone()
        {
            TinyMCEValueFormattersTestHelper.SingleWrappingParagraphWithClassIsLeftAlone(
                new NoParagraphPropertyValueFormatter());
        }

        [Test]
        public void Multiple_wrapping_paragraphs_are_left_alone()
        {
            TinyMCEValueFormattersTestHelper.MultipleWrappingParagraphsAreLeftAlone(
                 new NoParagraphPropertyValueFormatter());
        }

        [Test]
        public void Style_attribute_is_removed_from_ordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromOrderedLists(
                new NoParagraphPropertyValueFormatter());
        }

        [TestCase("lower-alpha")]
        [TestCase("lower-greek")]
        [TestCase("lower-roman")]
        [TestCase("upper-alpha")]
        [TestCase("upper-roman")]
        public void Permitted_style_attribute_is_converted_to_class_from_ordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnOrderedLists(
                new NoParagraphPropertyValueFormatter(), listStyleType);
        }

        [Test]
        public void Style_attribute_is_removed_from_unordered_lists()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromUnorderedLists(
                new NoParagraphPropertyValueFormatter());
        }

        [TestCase("circle")]
        [TestCase("square")]
        public void Permitted_style_attribute_is_converted_to_class_on_unordered_lists(string listStyleType)
        {
            TinyMCEValueFormattersTestHelper.TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(
                new NoParagraphPropertyValueFormatter(), listStyleType);
        }


        [Test]
        public void Style_attribute_is_removed_from_paragraphs()
        {
            TinyMCEValueFormattersTestHelper.TestStyleAttributeIsRemovedFromParagraphs(
                new NoParagraphPropertyValueFormatter());
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
                new NoParagraphPropertyValueFormatter(), styleAttribute, expectedClass);
        }
    }
}
