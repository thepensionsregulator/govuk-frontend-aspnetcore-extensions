using Microsoft.AspNetCore.Http;
using System.Web;
using ThePensionsRegulator.GovUk.Frontend.Umbraco.Caching;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Tests.Caching
{
    public class GovUkUmbracoStaticFileCachePolicyTests
    {
        private readonly GovUkUmbracoStaticFileCachePolicy _policy = new();

        [Theory]
        [InlineData("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/govuk-component-hfdksfhks.js", true)]
        [InlineData("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/package-version.generated-hjrkehwrk.js", true)]
        [InlineData("/App_Plugins/ThePensionsRegulator.GovUk.Frontend.Umbraco/example-helper-code-hdjskhfjds.js", true)]
        [InlineData("/ThePensionsRegulator.GovUk.Frontend.Umbraco/css/govuk-frontend.css?v=1.0.0", true)]
        [InlineData("/css/govuk-umbraco-backoffice.css?v=1.0.0", true)]
        [InlineData("/THEPENSIONSREGULATOR.GOVUK.FRONTEND.UMBRACO/CSS/GOVUK-FRONTEND.CSS?v=1.0.0", true)]

        // no path
        [InlineData("?v=1.0.0", false)]

        // wrong path
        [InlineData("/_content/OtherPackage/style.css?v=1.0.0", false)]
        [InlineData("/_content/ThePensionsRegulator/style.css?v=1.0.0", false)]
        [InlineData("/other-content/ThePensionsRegulator.GovUk.Frontend.Umbraco/govuk-component-hfjdkfs.js", false)]

        // no querystring
        [InlineData("/ThePensionsRegulator.GovUk.Frontend.Umbraco/css/govuk-frontend.css", false)]
        [InlineData("/css/govuk-umbraco-backoffice.css", false)]
        [InlineData("/other/file.js", false)]

        // wrong querystring
        [InlineData("/ThePensionsRegulator.GovUk.Frontend.Umbraco/css/govuk-frontend.css?other=value", false)]
        [InlineData("/css/govuk-umbraco-backoffice.css?v=", false)]
        public void IsImmutable_ReturnsExpectedResult(string path, bool expected)
        {
            // Arrange
            var url = new Uri("https://example.com" + path);
            var query = HttpUtility.ParseQueryString(url.Query);
            var dict = query.Keys.OfType<string>().ToDictionary(k => k, k => (Microsoft.Extensions.Primitives.StringValues)query[k]);

            // Act
            var result = _policy.IsImmutable(path, new QueryCollection(dict));

            // Assert
            Assert.Equal(expected, result);
        }
    }
}