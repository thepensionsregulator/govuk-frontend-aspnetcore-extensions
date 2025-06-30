using HtmlAgilityPack;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.PropertyEditors;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    internal class TinyMCEValueFormattersTestHelper
    {
        internal static void SingleWrappingParagraphWithNoClassIsRemoved(IPropertyValueFormatter formatter)
        {
            var html = "<p>Some content</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            Assert.AreEqual("Some content", result.ToHtmlString());
        }

        internal static void SingleWrappingParagraphWithClassIsLeftAlone(IPropertyValueFormatter formatter)
        {
            var html = "<p class=\"some-class\">Some content</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            Assert.AreEqual("<p class=\"some-class govuk-body\">Some content</p>", result.ToHtmlString());
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

        internal static void TestPermittedStyleAttributeIsConvertedToClassOnOrderedLists(IPropertyValueFormatter formatter, string listStyleType)
        {
            var html = $"<ol style=\"list-style-type: {listStyleType};\"><li>Item 1</li><li>Item 2</li></ol>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//ol").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes($"//ol[contains(@class,'govuk-list--{listStyleType}')]").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//ol[@style]"));
        }

        internal static void TestStyleAttributeIsRemovedFromUnorderedLists(IPropertyValueFormatter formatter)
        {
            var html = "<ul style=\"list-style-type: circle;\"><li>Item 1</li><li>Item 2</li></ul><ul style=\"color: red;\"><li>Item 3</li><li>Item 4</li></ul>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ul").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//ul[@style]"));
        }

        internal static void TestPermittedStyleAttributeIsConvertedToClassOnUnorderedLists(IPropertyValueFormatter formatter, string listStyleType)
        {
            var html = $"<ul style=\"list-style-type: {listStyleType};\"><li>Item 1</li><li>Item 2</li></ul>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//ul").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes($"//ul[contains(@class,'govuk-list--{listStyleType}')]").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//ul[@style]"));
        }

        internal static void TestStyleAttributeIsRemovedFromParagraphs(IPropertyValueFormatter formatter)
        {
            var html = "<p style=\"text-align: center;\">Example text</p><p style=\"color: red;\">Example text</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//p").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//p[@style]"));
        }

        internal static void TestPermittedStyleAttributeIsConvertedToClassOnParagraphs(IPropertyValueFormatter formatter, string styleAttribute, string expectedClass)
        {
            var html = $"<p style=\"{styleAttribute};\">Example text</p>";

            var result = (IHtmlEncodedString)formatter.FormatValue(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result.ToHtmlString());
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//p").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes($"//p[contains(@class,'{expectedClass}')]").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//p[@style]"));
        }
    }
}
