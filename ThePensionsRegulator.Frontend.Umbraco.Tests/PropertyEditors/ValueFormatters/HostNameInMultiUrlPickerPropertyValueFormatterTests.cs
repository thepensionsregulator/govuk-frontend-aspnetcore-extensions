using Microsoft.AspNetCore.Http;
using Moq;
using ThePensionsRegulator.Frontend.Umbraco.PropertyEditors.ValueFormatters;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.PropertyEditors.ValueFormatters
{
    [TestFixture]
    public class HostNameInMultiUrlPickerPropertyValueFormatterTests
    {
        [Test]
        public void Accepts_Link_as_input_and_replaces_link()
        {
            // Arrange
            var input = new Link { Url = "https://example.org" };
            var expected = "https://example.com";

            var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("https://example.org", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNameInMultiUrlPickerPropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var result = formatter.FormatValue(input);

            // Assert
            Assert.That(((Link?)result)?.Url, Is.EqualTo(expected));
        }

        [Test]
        public void Accepts_List_of_Link_as_input_and_replaces_all_links()
        {
            // Arrange
            var input = new List<Link> {
                new Link { Url = "https://example.org" },
                new Link { Url = "https://example.net" }
            };
            var expected = "https://example.com";

            var context = new UmbracoTestContext();
            var accessor = new Mock<IHttpContextAccessor>();
            accessor.Setup(x => x.HttpContext).Returns(context.HttpContext.Object);

            var hostUpdater = new Mock<IContextAwareHostUpdater>();
            hostUpdater.Setup(x => x.UpdateHost("https://example.org", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");
            hostUpdater.Setup(x => x.UpdateHost("https://example.net", context.HttpContext.Object.Request.Host.Host)).Returns("https://example.com");

            var formatter = new HostNameInMultiUrlPickerPropertyValueFormatter(accessor.Object, hostUpdater.Object);

            // Act
            var results = formatter.FormatValue(input);

            // Assert
            var links = results as IEnumerable<Link>;
            Assert.NotNull(links);
            foreach (var result in links!)
            {
                Assert.That(((Link?)result)?.Url, Is.EqualTo(expected));
            }
        }
    }
}
