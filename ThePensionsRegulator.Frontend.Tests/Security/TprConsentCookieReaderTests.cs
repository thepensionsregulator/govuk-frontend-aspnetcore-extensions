using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Moq;
using ThePensionsRegulator.Frontend.Security;

namespace ThePensionsRegulator.Frontend.Tests.Security
{
    public class TprConsentCookieReaderTests
    {
        private readonly HttpContext _context = new DefaultHttpContext();
        private readonly Mock<IHttpContextAccessor> _contextAccessor = new();

        public TprConsentCookieReaderTests()
        {
            _contextAccessor.Setup(x => x.HttpContext).Returns(_context);
        }

        private static IRequestCookieCollection MockRequestCookieCollection(string key, string value)
        {
            var requestFeature = new HttpRequestFeature();
            var featureCollection = new FeatureCollection();

            requestFeature.Headers = new HeaderDictionary();
            requestFeature.Headers.Append(HeaderNames.Cookie, new StringValues(key + "=" + value));

            featureCollection.Set<IHttpRequestFeature>(requestFeature);

            var cookiesFeature = new RequestCookiesFeature(featureCollection);

            return cookiesFeature.Cookies;
        }

        [Fact]
        public void No_cookie_returns_false()
        {
            // Arrange
            var reader = new TprConsentCookieReader(_contextAccessor.Object);

            // Act
            var result1 = reader.HasConsent();
            var result2 = reader.HasConsent(TprConsentCookieReader.TPR_CONSENT_CATEGORY_PERFORMANCE);

            // Assert
            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Reject_all_returns_false()
        {
            // Arrange
            _context.Request.Cookies = MockRequestCookieCollection(TprConsentCookieReader.TPR_CONSENT_COOKIE_NAME, "Reject|");
            var reader = new TprConsentCookieReader(_contextAccessor.Object);

            // Act
            var result1 = reader.HasConsent();
            var result2 = reader.HasConsent(TprConsentCookieReader.TPR_CONSENT_CATEGORY_PERFORMANCE);

            // Assert
            Assert.False(result1);
            Assert.False(result2);
        }

        [Fact]
        public void Accept_all_returns_true()
        {
            // Arrange
            _context.Request.Cookies = MockRequestCookieCollection(TprConsentCookieReader.TPR_CONSENT_COOKIE_NAME, "All|");
            var reader = new TprConsentCookieReader(_contextAccessor.Object);

            // Act
            var result1 = reader.HasConsent();
            var result2 = reader.HasConsent(TprConsentCookieReader.TPR_CONSENT_CATEGORY_PERFORMANCE);

            // Assert
            Assert.True(result1);
            Assert.True(result2);
        }

        [Theory]
        [InlineData(TprConsentCookieReader.TPR_CONSENT_CATEGORY_PERFORMANCE, true)]
        [InlineData(TprConsentCookieReader.TPR_CONSENT_CATEGORY_FUNCTIONALITY, true)]
        [InlineData(TprConsentCookieReader.TPR_CONSENT_CATEGORY_TARGETING, false)]
        public void Partial_consent_returns_correct_response(string category, bool expectedResponse)
        {
            // Arrange
            _context.Request.Cookies = MockRequestCookieCollection(TprConsentCookieReader.TPR_CONSENT_COOKIE_NAME, "performance=On|functionality=On|targeting=Off|");
            var reader = new TprConsentCookieReader(_contextAccessor.Object);

            // Act
            var result = reader.HasConsent(category);

            // Assert
            Assert.Equal(expectedResponse, result);
        }
    }
}
