using Microsoft.AspNetCore.Http;
using Moq;
using ThePensionsRegulator.GovUk.Frontend.Caching;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.Caching
{
    public class CacheStaticFilesMiddlewareTests
    {
        private readonly Mock<RequestDelegate> _nextMiddleware = new();
        private readonly Mock<IStaticFileCachePolicy> _cachePolicy = new();

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task InvokeAsync_WithCacheablePath_SetsCacheControlHeaders(bool expectCacheHeaders)
        {
            // Arrange
            const string path = "/govuk/style.css";
            _cachePolicy.Setup(p => p.IsImmutable(path, It.IsAny<IQueryCollection>())).Returns(expectCacheHeaders);

            var middleware = new CacheStaticFilesMiddleware(_nextMiddleware.Object, new[] { _cachePolicy.Object });
            var context = new DefaultHttpContext();
            context.Request.Path = path;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(expectCacheHeaders, context.Response.Headers.ContainsKey("Cache-Control"));
            if (expectCacheHeaders)
            {
                var cacheControlValue = context.Response.Headers["Cache-Control"].ToString();
                Assert.Contains("public", cacheControlValue);
                Assert.Contains("max-age=31536000", cacheControlValue);
                Assert.Contains("immutable", cacheControlValue);
            }
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public async Task InvokeAsync_WithMultiplePolicies_AnyMatchingPolicySetsHeaders(bool policy1Result, bool policy2Result)
        {
            // Arrange
            const string path = "/file.css";
            var policy1 = new Mock<IStaticFileCachePolicy>();
            var policy2 = new Mock<IStaticFileCachePolicy>();

            policy1.Setup(p => p.IsImmutable(path, It.IsAny<IQueryCollection>())).Returns(policy1Result);
            policy2.Setup(p => p.IsImmutable(path, It.IsAny<IQueryCollection>())).Returns(policy2Result);

            var middleware = new CacheStaticFilesMiddleware(_nextMiddleware.Object, new[] { policy1.Object, policy2.Object });
            var context = new DefaultHttpContext();
            context.Request.Path = path;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            Assert.Equal(policy1Result || policy2Result, context.Response.Headers.ContainsKey("Cache-Control"));
        }

        [Fact]
        public async Task InvokeAsync_CallsNextMiddleware()
        {
            // Arrange
            _cachePolicy.Setup(p => p.IsImmutable(It.IsAny<string>(), It.IsAny<IQueryCollection>())).Returns(false);

            var middleware = new CacheStaticFilesMiddleware(_nextMiddleware.Object, new[] { _cachePolicy.Object });
            var context = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _nextMiddleware.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_WithNullPath_HandlesGracefully()
        {
            // Arrange
            _cachePolicy.Setup(p => p.IsImmutable(string.Empty, It.IsAny<IQueryCollection>())).Returns(false);

            var middleware = new CacheStaticFilesMiddleware(_nextMiddleware.Object, new[] { _cachePolicy.Object });
            var context = new DefaultHttpContext();
            context.Request.Path = PathString.Empty;

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            _nextMiddleware.Verify(next => next(context), Times.Once);
        }
    }
}