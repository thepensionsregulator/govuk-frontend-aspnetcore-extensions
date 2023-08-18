using GovUk.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Strings;

namespace GovUk.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [TestFixture]
    public class HostNamePropertyValueFormatterTests
    {
        [Test]
        public void Accepts_string_or_HtmlEncodedString_as_input_and_replaces_links()
        {
            // Arrange
            const string INPUT = "<p><a href=\"https://example.org\">Example</a><a href=\"https://example.org\">Example</a></p>";
            const string EXPECTED = "<p><a href=\"https://example.com\">Example</a><a href=\"https://example.com\">Example</a></p>";

            var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("https://example.org", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNamePropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var resultOfString = formatter.FormatValue(INPUT);
            var resultOfHtmlEncodedString = formatter.FormatValue(new HtmlEncodedString(INPUT));

            // Assert
            Assert.That(((HtmlEncodedString)resultOfString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
            Assert.That(((HtmlEncodedString)resultOfHtmlEncodedString)?.ToHtmlString(), Is.EqualTo(EXPECTED));
        }
    }
}
