using Microsoft.AspNetCore.Http;
using System.Web;
using ThePensionsRegulator.Frontend.Caching;

namespace ThePensionsRegulator.Frontend.Tests.Caching
{
    public class TprStaticFileCachePolicyTests
    {
        private readonly TprStaticFileCachePolicy _policy = new();

        [Theory]
        [InlineData("/ThePensionsRegulator.Frontend/style.css?v=1.0.0", true)]
        [InlineData("/ThePensionsRegulator.Frontend/script.js?v=1.0.0", true)]
        [InlineData("/ThePensionsRegulator.Frontend/image.png?v=1.0.0", true)]
        [InlineData("/ThePensionsRegulator.Frontend/open-sans.woff?v=1.0.0", true)]
        [InlineData("/THEPENSIONSREGULATOR.FRONTEND/STYLE.CSS?v=1.0.0", true)]

        // no path
        [InlineData("?v=1.0.0", false)]

        // wrong path
        [InlineData("/_content/OtherPackage/style.css?v=1.0.0", false)]
        [InlineData("/_content/ThePensionsRegulator/style.css?v=1.0.0", false)]
        [InlineData("/other-content/ThePensionsRegulator.Frontend/style.css?v=1.0.0", false)]

        // no querystring
        [InlineData("/ThePensionsRegulator.Frontend/style.css", false)]
        [InlineData("/other/file.js", false)]

        // wrong querystring
        [InlineData("/ThePensionsRegulator.Frontend/style.css?other=value", false)]
        [InlineData("/ThePensionsRegulator.Frontend/image.png?v=", false)]
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