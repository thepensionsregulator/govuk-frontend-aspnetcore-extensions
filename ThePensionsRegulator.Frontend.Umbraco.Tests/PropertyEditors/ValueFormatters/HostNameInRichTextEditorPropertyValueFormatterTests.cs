using Microsoft.AspNetCore.Http;
using Moq;
using ThePensionsRegulator.Frontend.Services;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Strings;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [Collection("UmbracoTests")]
    public class HostNameInRichTextEditorPropertyValueFormatterTests
    {
        [Fact]
        public void Accepts_string_or_HtmlEncodedString_as_input_and_replaces_links()
        {
            // Arrange
            const string INPUT = "<p><a href=\"https://example.org\">Example</a><a href=\"https://example.org\">Example</a></p>";
            const string EXPECTED = "<p><a href=\"https://example.com\">Example</a><a href=\"https://example.com\">Example</a></p>";

            using var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("https://example.org", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNameInRichTextEditorPropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfString)?.ToHtmlString());
            Assert.Equal(EXPECTED, ((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString());
        }

        [Fact]
        public void Ignores_anchor_link_targets()
        {
            // Arrange
            const string INPUT = "<p><a id='some-id'>Example</a></p>";

            using var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("https://example.org", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNameInRichTextEditorPropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var result = formatter.FormatValue(INPUT);

            // Assert
            Assert.Equal(INPUT, ((HtmlEncodedString)result)?.ToHtmlString());
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("  ")]
        public void Ignores_empty_href(string hrefValue)
        {
            // Arrange
            string INPUT = $"<p><a href='{hrefValue}'>Example</a></p>";

            using var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNameInRichTextEditorPropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var result = formatter.FormatValue(INPUT);

            // Assert
            Assert.Equal(INPUT, ((HtmlEncodedString)result)?.ToHtmlString());
        }
    }
}
