using GovUk.Frontend.Umbraco.Typography;
using HtmlAgilityPack;
using NUnit.Framework;

namespace GovUk.Frontend.Umbraco.Tests
{
    public class GovUkTypographyTests
    {
        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void Null_or_whitespace_returns_empty_string(string? html)
        {
            var result = GovUkTypography.Apply(html);
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Link_class_is_added()
        {
            var html = "<p><a href=\"https://example.org\">Some link</a></p>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//a[@class='govuk-link']").Count);
        }

        [Test]
        public void Inverse_link_class_is_added_for_dark_backgrounds()
        {
            var html = "<p><a href=\"https://example.org\">Some link</a></p>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { BackgroundType = BackgroundType.Dark });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//a[contains(@class,'govuk-link')]").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//a[contains(@class,'govuk-link--inverse')]").Count);
        }


        [Test]
        public void Heading_medium_class_is_added_to_H2()
        {
            var html = "<h2>Heading</h2><h2>Another heading</h2>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-m']").Count);
        }

        [Test]
        public void Heading_medium_class_is_not_added_to_H2_if_another_heading_class_is_already_present()
        {
            var html = "<h2 class=\"govuk-heading-s\">Heading</h2><h2>Another heading</h2>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-s']").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-m']").Count);
        }

        [Test]
        public void Heading_small_class_is_added_to_H3()
        {
            var html = "<h3>Heading</h3><h3>Another heading</h3>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//h3[@class='govuk-heading-s']").Count);
        }

        [Test]
        public void Heading_small_class_is_not_added_to_H3_if_another_heading_class_is_already_present()
        {
            var html = "<h3 class=\"govuk-heading-m\">Heading</h3><h3>Another heading</h3>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//h3[@class='govuk-heading-s']").Count);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//h3[@class='govuk-heading-m']").Count);
        }

        [Test]
        public void Body_class_is_added_to_paragraphs()
        {
            var html = "<p>Content</p><p>More content</p>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//p[@class='govuk-body']").Count);
        }

        [Test]
        public void List_classes_are_applied_to_unordered_lists()
        {
            var html = "<ul><li>Item 1</li><li>Item 2</li></ul><ul><li>Item 3</li><li>Item 4</li></ul>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ul[contains(@class,'govuk-list')]").Count);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ul[contains(@class,'govuk-list--bullet')]").Count);
        }

        [Test]
        public void List_classes_are_applied_to_ordered_lists()
        {
            var html = "<ol><li>Item 1</li><li>Item 2</li></ol><ol><li>Item 3</li><li>Item 4</li></ol>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ol[contains(@class,'govuk-list')]").Count);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//ol[contains(@class,'govuk-list--number')]").Count);
        }

        [Test]
        public void Single_wrapping_paragraph_is_removed_if_requested()
        {
            var html = "<p>Some content</p>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { RemoveWrappingParagraph = true });

            Assert.AreEqual("Some content", result);
        }

        [Test]
        public void Single_wrapping_paragraph_is_not_removed_if_not_requested()
        {
            var html = "<p>Some content</p>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(1, doc.DocumentNode.SelectNodes("//p").Count);
        }

        [Test]
        public void Multiple_wrapping_paragraphs_are_left_alone_by_removal_request()
        {
            var html = "<p>Some content</p><p>Some content</p>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { RemoveWrappingParagraph = true });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.AreEqual(2, doc.DocumentNode.SelectNodes("//p").Count);
        }
    }
}