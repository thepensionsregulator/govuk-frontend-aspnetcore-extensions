using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using DI = Umbraco.Cms.Core.DependencyInjection;

namespace ThePensionsRegulator.Umbraco.Testing.Tests
{
    public class UmbracoTestContextTests
    {
        [Fact]
        public void Can_add_content_type()
        {
            // Arrange
            var testContext = new UmbracoTestContext();
            const string contentTypeAlias = "MyContentType2";

            // Act
            testContext.SetupContentType(contentTypeAlias);

            // Assert
            var contentType = testContext.ContentTypes[contentTypeAlias]?.Object;
            Assert.NotNull(contentType);
            Assert.Equal(1, contentType.Id);
            Assert.NotEqual(default(Guid).ToString(), contentType.Key.ToString());
            Assert.Equal(contentTypeAlias, contentType.Alias);

            Assert.Equal(contentType, testContext.PublishedContentCache.Object.GetContentType(contentTypeAlias));
        }

        [Fact]
        public void Can_mock_authenticated_HttpContext_User()
        {
            var testContext = new UmbracoTestContext();

            testContext.CurrentIdentity.Setup(x => x.IsAuthenticated).Returns(true);

            Assert.True(testContext.HttpContext.Object.User.Identity?.IsAuthenticated ?? false);
        }

        [Fact]
        public void Can_mock_authenticated_HttpContext_User_with_claims()
        {
            var testContext = new UmbracoTestContext();

            var identity = new ClaimsIdentity(new Claim[] { new Claim("type1", "value1"), new Claim("type2", "value2") }, "any string makes IsAuthenticated return true");
            testContext.CurrentPrincipal = new GenericPrincipal(identity, Array.Empty<string>());

            Assert.True(testContext.HttpContext.Object.User.Claims.Count() > 0);
            Assert.True(testContext.HttpContext.Object.User.Identity?.IsAuthenticated ?? false);
            Assert.Equal(testContext.CurrentPrincipal, Thread.CurrentPrincipal);
        }

        private class DummyController : Controller
        { }

        [Fact]
        public void Can_call_claims_from_controller()
        {
            var testContext = new UmbracoTestContext();

            var identity = new ClaimsIdentity(new Claim[] { new Claim("type1", "value1"), new Claim("type2", "value2") }, "any string makes IsAuthenticated return true");
            testContext.CurrentPrincipal = new GenericPrincipal(identity, Array.Empty<string>());

            var controllerContext = testContext.ControllerContext;

            var controller = new DummyController();
            controller.ControllerContext = controllerContext;

            var claims = controller.User.Claims;

            Assert.Equal(2, claims.Count());
        }

        [Fact]
        public void Can_set_and_get_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";
            const string data = "hello world";

            testContext.Session.Object.SetString(key, data);

            Assert.Contains(key, testContext.Session.Object.Keys);

            var result = testContext.Session.Object.GetString(key);
            Assert.Equal(data, result);
        }

        [Fact]
        public void Key_not_in_session_returns_null()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";

            Assert.DoesNotContain(key, testContext.Session.Object.Keys);

            var result = testContext.Session.Object.Get(key);
            Assert.Null(result);
        }

        [Fact]
        public void Can_remove_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";
            const string data = "hello world";

            testContext.Session.Object.SetString(key, data);
            testContext.Session.Object.Remove(key);

            Assert.DoesNotContain(key, testContext.Session.Object.Keys);
        }

        [Fact]
        public void Can_clear_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key1 = "test1";
            const string key2 = "test2";
            const string data = "hello world";

            testContext.Session.Object.SetString(key1, data);
            testContext.Session.Object.SetString(key2, data);
            testContext.Session.Object.Clear();

            Assert.Empty(testContext.Session.Object.Keys);
        }

        [Fact]
        public void Does_not_replace_static_service_provider()
        {
            var originalProvider = Mock.Of<IServiceProvider>();

            DI.StaticServiceProvider.Instance = originalProvider;

            _ = new UmbracoTestContext();

            Assert.Equal(originalProvider, DI.StaticServiceProvider.Instance);
        }
    }
}