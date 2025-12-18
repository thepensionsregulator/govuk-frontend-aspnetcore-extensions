using GovUk.Frontend.AspNetCore.Extensions.Caching;
using Microsoft.AspNetCore.Http;
using System.Web;

namespace GovUk.Frontend.AspNetCore.Extensions.UnitTests.Caching
{
    public class GovUkStaticFileCachePolicyTests
    {
        private readonly GovUkStaticFileCachePolicy _policy = new();

        [Theory]
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/style.css?v=1.0.0", true)]
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/script.js?v=1.0.0", true)]
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/image.png?v=1.0.0", true)]
        [InlineData("/_CONTENT/THEPENSIONSREGULATOR.GOVUK.FRONTEND/STYLE.CSS?v=1.0.0", true)]

        // no path
        [InlineData("?v=1.0.0", false)]

        // wrong path
        [InlineData("/_content/OtherPackage/style.css?v=1.0.0", false)]
        [InlineData("/_content/ThePensionsRegulator/style.css?v=1.0.0", false)]
        [InlineData("/other-content/ThePensionsRegulator.GovUk.Frontend/style.css?v=1.0.0", false)]

        // no querystring
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/style.css", false)]
        [InlineData("/govuk/style.css", false)]
        [InlineData("/other/file.js", false)]

        // wrong querystring
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/style.css?other=value", false)]
        [InlineData("/_content/ThePensionsRegulator.GovUk.Frontend/image.png?v=", false)]
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