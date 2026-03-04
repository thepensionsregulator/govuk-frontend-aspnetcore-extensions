using GovUk.Frontend.AspNetCore.Extensions.Typography;
using HtmlAgilityPack;
namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests
{
    public class GovUkTypographyTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public void Null_or_whitespace_returns_empty_string(string? html)
        {
            var result = GovUkTypography.Apply(html);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Link_class_is_added()
        {
            var html = "<p><a href=\"https://example.org\">Some link</a></p>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Single(doc.DocumentNode.SelectNodes("//a[@class='govuk-link']"));
        }

        [Fact]
        public void Inverse_link_class_is_added_for_dark_backgrounds()
        {
            var html = "<p><a href=\"https://example.org\">Some link</a></p>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { BackgroundType = BackgroundType.Dark });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Single(doc.DocumentNode.SelectNodes("//a[contains(@class,'govuk-link')]"));
            Assert.Single(doc.DocumentNode.SelectNodes("//a[contains(@class,'govuk-link--inverse')]"));
        }

        [Fact]
        public void Heading_medium_class_is_added_to_H2()
        {
            var html = "<h2>Heading</h2><h2>Another heading</h2>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-m']").Count);
        }

        [Fact]
        public void Heading_medium_class_is_not_added_to_H2_if_another_heading_class_is_already_present()
        {
            var html = "<h2 class=\"govuk-heading-s\">Heading</h2><h2>Another heading</h2>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Single(doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-s']"));
            Assert.Single(doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-m']"));
        }

        [Fact]
        public void Custom_heading_class_is_added_to_H2()
        {
            var html = "<h2>Heading</h2><h2>Another heading</h2>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { HeadingClasses = new HeadingClasses { Heading2 = "custom" } });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h2[@class='custom']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//h2[@class='govuk-heading-m']"));
        }

        [Theory]
        [InlineData("h3")]
        [InlineData("h4")]
        [InlineData("h5")]
        [InlineData("h6")]
        public void Heading_small_class_is_added_to_lower_heading_levels(string headingLevel)
        {
            var html = $"<{headingLevel}>Heading</{headingLevel}><{headingLevel}>Another heading</{headingLevel}>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes($"//{headingLevel}[@class='govuk-heading-s']").Count);
        }

        [Theory]
        [InlineData("h3")]
        [InlineData("h4")]
        [InlineData("h5")]
        [InlineData("h6")]
        public void Heading_small_class_is_not_added_to_lower_heading_levels_if_another_heading_class_is_already_present(string headingLevel)
        {
            var html = $"<{headingLevel} class=\"govuk-heading-m\">Heading</{headingLevel}><{headingLevel}>Another heading</{headingLevel}>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Single(doc.DocumentNode.SelectNodes($"//{headingLevel}[@class='govuk-heading-s']"));
            Assert.Single(doc.DocumentNode.SelectNodes($"//{headingLevel}[@class='govuk-heading-m']"));
        }

        [Fact]
        public void Custom_heading_class_is_added_to_H3()
        {
            var html = "<h3>Heading</h3><h3>Another heading</h3>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { HeadingClasses = new HeadingClasses { Heading3 = "custom" } });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h3[@class='custom']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//h3[@class='govuk-heading-s']"));
        }

        [Fact]
        public void Custom_heading_class_is_added_to_H4()
        {
            var html = "<h4>Heading</h4><h4>Another heading</h4>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { HeadingClasses = new HeadingClasses { Heading4 = "custom" } });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h4[@class='custom']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//h4[@class='govuk-heading-s']"));
        }

        [Fact]
        public void Custom_heading_class_is_added_to_H5()
        {
            var html = "<h5>Heading</h5><h5>Another heading</h5>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { HeadingClasses = new HeadingClasses { Heading5 = "custom" } });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h5[@class='custom']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//h5[@class='govuk-heading-s']"));
        }

        [Fact]
        public void Custom_heading_class_is_added_to_H6()
        {
            var html = "<h6>Heading</h6><h6>Another heading</h6>";

            var result = GovUkTypography.Apply(html, new TypographyOptions { HeadingClasses = new HeadingClasses { Heading6 = "custom" } });

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//h6[@class='custom']").Count);
            Assert.Null(doc.DocumentNode.SelectNodes("//h6[@class='govuk-heading-s']"));
        }

        [Fact]
        public void Body_class_is_added_to_paragraphs()
        {
            var html = "<p>Content</p><p>More content</p>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//p[@class='govuk-body']").Count);
        }

        [Fact]
        public void List_classes_are_applied_to_unordered_lists()
        {
            var html = "<ul><li>Item 1</li><li>Item 2</li></ul><ul><li>Item 3</li><li>Item 4</li></ul>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//ul[contains(@class,'govuk-list')]").Count);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//ul[contains(@class,'govuk-list--bullet')]").Count);
        }

        [Fact]
        public void List_classes_are_applied_to_ordered_lists()
        {
            var html = "<ol><li>Item 1</li><li>Item 2</li></ol><ol><li>Item 3</li><li>Item 4</li></ol>";

            var result = GovUkTypography.Apply(html);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//ol[contains(@class,'govuk-list')]").Count);
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//ol[contains(@class,'govuk-list--number')]").Count);
        }

        [Fact]
        public void Table_classes_are_added()
        {
            var html = @"<table>
                <caption>Dates and amounts</caption>
                <thead>
                <tr>
                    <th scope=""col"">Date</th>
                    <th scope=""col"">Amount</th>
                </tr>
                </thead>
                <tbody>
                <tr>
                    <th scope=""row"">First 6 weeks</th>
                    <td>£109.80 per week</td>
                </tr>
                <tr>
                    <th scope=""row"">Next 33 weeks</th>
                    <td>£109.80 per week</td>
                </tr>
                <tr>
                    <th scope=""row"">Total estimated pay</th>
                    <td>£4,282.20</td>
                </tr>
                </tbody>
            </table>";

            var result = GovUkTypography.Apply(html, new TypographyOptions());

            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            Assert.Single(doc.DocumentNode.SelectNodes("//table[contains(@class,'govuk-table')]"));
            Assert.Single(doc.DocumentNode.SelectNodes("//caption[contains(@class,'govuk-table__caption')]"));
            Assert.Single(doc.DocumentNode.SelectNodes("//thead[contains(@class,'govuk-table__head')]"));
            Assert.Single(doc.DocumentNode.SelectNodes("//thead/tr[contains(@class,'govuk-table__row')]"));
            Assert.Equal(2, doc.DocumentNode.SelectNodes("//thead//th[contains(@class,'govuk-table__header')]").Count);
            Assert.Single(doc.DocumentNode.SelectNodes("//tbody[contains(@class,'govuk-table__body')]"));
            Assert.Equal(3, doc.DocumentNode.SelectNodes("//tbody/tr[contains(@class,'govuk-table__row')]").Count);
            Assert.Equal(3, doc.DocumentNode.SelectNodes("//tbody/tr/th[contains(@class,'govuk-table__header')]").Count);
            Assert.Equal(3, doc.DocumentNode.SelectNodes("//tbody/tr/td[contains(@class,'govuk-table__cell')]").Count);

        }

        [Fact]
        public void HeadingLevels_scales_down_from_xl_when_xl_passed()
        {
            var result = GovUkTypography.HeadingClasses("govuk-heading-xl");

            Assert.Equal("govuk-caption-xl", result.Caption);
            Assert.Equal("govuk-heading-xl", result.Heading1);
            Assert.Equal("govuk-label--xl", result.LabelAsHeading1);
            Assert.Equal("govuk-fieldset__legend--xl", result.LegendAsHeading1);
            Assert.Equal("govuk-heading-l", result.Heading2);
            Assert.Equal("govuk-heading-m", result.Heading3);
            Assert.Equal("govuk-heading-s", result.Heading4);
            Assert.Equal("govuk-heading-s", result.Heading5);
            Assert.Equal("govuk-heading-s", result.Heading6);
        }

        [Theory]
        [InlineData("govuk-heading-l")]
        [InlineData("govuk-heading-m")]
        [InlineData("govuk-heading-s")]
        [InlineData("something-else")]
        [InlineData("")]
        [InlineData(null)]
        public void HeadingLevels_scales_down_from_l_when_any_other_value_passed(string? value)
        {
            var result = GovUkTypography.HeadingClasses(value);
            Assert.Equal("govuk-caption-l", result.Caption);
            Assert.Equal("govuk-heading-l", result.Heading1);
            Assert.Equal("govuk-label--l", result.LabelAsHeading1);
            Assert.Equal("govuk-fieldset__legend--l", result.LegendAsHeading1);
            Assert.Equal("govuk-heading-m", result.Heading2);
            Assert.Equal("govuk-heading-s", result.Heading3);
            Assert.Equal("govuk-heading-s", result.Heading4);
            Assert.Equal("govuk-heading-s", result.Heading5);
            Assert.Equal("govuk-heading-s", result.Heading6);
        }
    }
}