using HtmlAgilityPack;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    internal class TinyMCEValueFormattersTestHelper
    {
        internal static void SingleWrappingParagraphIsRemoved(IPropertyValueFormatter formatter)
        {
            var html = "<p>Some content</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            Assert.AreEqual("Some content", result.ToHtmlString());
        }

        internal static void MultipleWrappingParagraphsAreLeftAlone(IPropertyValueFormatter formatter)
        {
            var html = "<p>Some content</p><p>Some content</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//p").Count);
        }

        internal static void TestStyleAttributeIsRemovedFromOrderedLists(IPropertyValueFormatter formatter)
        {
            var html = "<ol style=\"list-style-type: lower-alpha;\"><li>Item 1</li><li>Item 2</li></ol><ol style=\"color: red;\"><li>Item 3</li><li>Item 4</li></ol>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ol").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//ol[@style]"));
        }

        internal static void TestPermittedStyleAttributeIsConvertedToClassFromOrderedLists(IPropertyValueFormatter formatter, string listStyleType)
        {
            var html = $"<ol style=\"list-style-type: {listStyleType};\"><li>Item 1</li><li>Item 2</li></ol>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//ol").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes($"//ol[contains(@class,'govuk-list--{listStyleType}')]").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//ol[@style]"));
        }
    }
}
