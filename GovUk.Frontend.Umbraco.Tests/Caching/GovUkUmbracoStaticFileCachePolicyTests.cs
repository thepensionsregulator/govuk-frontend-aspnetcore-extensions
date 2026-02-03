using GovUk.Frontend.Umbraco.Caching;
using Microsoft.AspNetCore.Http;
using NUnit.Framework;
using System.Web;

namespace GovUk.Frontend.Umbraco.Tests.Caching
{
    public class GovUkUmbracoStaticFileCachePolicyTests
    {
        private readonly GovUkUmbracoStaticFileCachePolicy _policy = new();

        [Test]
        [TestCase("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/govuk-component-hfdksfhks.js", true)]
        [TestCase("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/package-version.generated-hjrkehwrk.js", true)]
        [TestCase("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/example-helper-code-hdjskhfjds.js", true)]
        [TestCase("/govuk/govuk-frontend.css?v=1.0.0", true)]
        [TestCase("/css/govuk-umbraco-backoffice.css?v=1.0.0", true)]
        [TestCase("/GOVUK/GOVUK-FRONTEND.CSS?v=1.0.0", true)]

        // no path
        [TestCase("?v=1.0.0", false)]

        // wrong path
        [TestCase("/_content/OtherPackage/style.css?v=1.0.0", false)]
        [TestCase("/_content/ThePensionsRegulator/style.css?v=1.0.0", false)]
        [TestCase("/other-content/ThePensionsRegulator.GovUk.Frontend.Umbraco/govuk-component-hfjdkfs.js", false)]

        // no querystring
        [TestCase("/govuk/govuk-frontend.css", false)]
        [TestCase("/css/govuk-umbraco-backoffice.css", false)]
        [TestCase("/other/file.js", false)]

        // wrong querystring
        [TestCase("/govuk/govuk-frontend.css?other=value", false)]
        [TestCase("/css/govuk-umbraco-backoffice.css?v=", false)]
        public void IsImmutable_ReturnsExpectedResult(string path, bool expected)
        {
            // Arrange
            var url = new Uri("https://example.com" + path);
            var query = HttpUtility.ParseQueryString(url.Query);
            var dict = query.Keys.OfType<string>().ToDictionary(k => k, k => (Microsoft.Extensions.Primitives.StringValues)query[k]);

            // Act
            var result = _policy.IsImmutable(path, new QueryCollection(dict));

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}