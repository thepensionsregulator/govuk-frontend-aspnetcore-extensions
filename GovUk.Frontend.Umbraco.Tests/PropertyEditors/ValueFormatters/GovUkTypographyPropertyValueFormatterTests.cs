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
        [TestCase(Constants.PropertyEditors.Aliases.TinyMce, true)]
        [TestCase(PropertyEditorAliases.GovUkInlineRichText, false)]
        [TestCase(PropertyEditorAliases.GovUkInlineInverseRichText, false)]
        [TestCase(PropertyEditorAliases.TprHeaderFooterRichText, false)]
        public void Applies_only_to_correct_rich_text_property_editor(string propertyEditorAlias, bool expected)
        {
            // Arrange
            var formatter = new GovUkTypographyPropertyValueFormatter();
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
            const string EXPECTED = "<p class=\"govuk-body\">Example</p>";
            var formatter = new GovUkTypographyPropertyValueFormatter();

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.That(((HtmlEncodedString)resultOfString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
            Assert.That(((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
        }
    }
}
