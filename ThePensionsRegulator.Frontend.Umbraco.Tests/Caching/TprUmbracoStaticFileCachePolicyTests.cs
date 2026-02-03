using Microsoft.AspNetCore.Http;
using System.Web;
using ThePensionsRegulator.Frontend.Umbraco.Caching;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests.Caching
{
    public class TprUmbracoStaticFileCachePolicyTests
    {
        private readonly TprUmbracoStaticFileCachePolicy _policy = new();

        [Theory]
        [InlineData("/App_Plugins/ThePensionsRegulator.Frontend.Umbraco/tpr-component-hfdksfhks.js", true)]
        [InlineData("/App_Plugins/ThePensionsRegulator.Frontend.Umbraco/package-version.generated-hjrkehwrk.js", true)]
        [InlineData("/App_Plugins/ThePensionsRegulator.Frontend.Umbraco/example-helper-code-hdjskhfjds.js", true)]
        [InlineData("/tpr/tpr.css?v=1.0.0", true)]
        [InlineData("/TPR/TPR.CSS?v=1.0.0", true)]

        // no path
        [InlineData("?v=1.0.0", false)]

        // wrong path
        [InlineData("/_content/OtherPackage/style.css?v=1.0.0", false)]
        [InlineData("/_content/ThePensionsRegulator/style.css?v=1.0.0", false)]
        [InlineData("/other-content/ThePensionsRegulator.Frontend.Umbraco/tpr-component-hfjdkfs.js", false)]

        // no querystring
        [InlineData("/tpr/tpr.css", false)]
        [InlineData("/other/file.js", false)]

        // wrong querystring
        [InlineData("/tpr/tpr.css?other=value", false)]
        [InlineData("/tpr/tpr.css?v=", false)]
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