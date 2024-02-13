using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace ThePensionsRegulator.Umbraco.Testing.Tests
{
    [TestFixture]
    public class UmbracoTestContextTests
    {
        [Test]
        public void Can_add_content_type()
        {
            // Arrange
            var testContext = new UmbracoTestContext();
            const string contentTypeAlias = "MyContentType2";

            // Act
            testContext.SetupContentType(contentTypeAlias);

            // Assert
            var contentType = testContext.ContentTypes[contentTypeAlias]?.Object;
            Assert.That(contentType, Is.Not.Null);
            Assert.That(contentType.Id, Is.EqualTo(1));
            Assert.That(contentType.Key.ToString(), Is.Not.EqualTo(default(Guid).ToString()));
            Assert.That(contentType.Alias, Is.EqualTo(contentTypeAlias));

            Assert.That(testContext.PublishedContentCache.Object.GetContentType(contentTypeAlias), Is.EqualTo(contentType));
        }

        [Test]
        public void Can_mock_authenticated_HttpContext_User()
        {
            var testContext = new UmbracoTestContext();

            testContext.CurrentIdentity.Setup(x => x.IsAuthenticated).Returns(true);

            Assert.True(testContext.HttpContext.Object.User.Identity?.IsAuthenticated ?? false);
        }

        [Test]
        public void Can_mock_authenticated_HttpContext_User_with_claims()
        {
            var testContext = new UmbracoTestContext();

            var identity = new ClaimsIdentity(new Claim[] { new Claim("type1", "value1"), new Claim("type2", "value2") }, "any string makes IsAuthenticated return true");
            testContext.CurrentPrincipal = new GenericPrincipal(identity, Array.Empty<string>());

            Assert.True(testContext.HttpContext.Object.User.Claims.Count() > 0);
            Assert.True(testContext.HttpContext.Object.User.Identity?.IsAuthenticated ?? false);
            Assert.That(testContext.CurrentPrincipal, Is.EqualTo(Thread.CurrentPrincipal));
        }

        private class DummyController : Controller
        { }

        [Test]
        public void Can_call_claims_from_controller()
        {
            var testContext = new UmbracoTestContext();

            var identity = new ClaimsIdentity(new Claim[] { new Claim("type1", "value1"), new Claim("type2", "value2") }, "any string makes IsAuthenticated return true");
            testContext.CurrentPrincipal = new GenericPrincipal(identity, Array.Empty<string>());

            var controllerContext = testContext.ControllerContext;

            var controller = new DummyController();
            controller.ControllerContext = controllerContext;

            var claims = controller.User.Claims;

            Assert.That(claims.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Can_set_and_get_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";
            const string data = "hello world";

            testContext.Session.Object.SetString(key, data);

            Assert.That(testContext.Session.Object.Keys.Contains(key), Is.True);

            var result = testContext.Session.Object.GetString(key);
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void Key_not_in_session_returns_null()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";

            Assert.That(testContext.Session.Object.Keys.Contains(key), Is.False);

            var result = testContext.Session.Object.Get(key);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Can_remove_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key = "test";
            const string data = "hello world";

            testContext.Session.Object.SetString(key, data);
            testContext.Session.Object.Remove(key);

            Assert.That(testContext.Session.Object.Keys.Contains(key), Is.False);
        }

        [Test]
        public void Can_clear_session_data()
        {
            var testContext = new UmbracoTestContext();
            const string key1 = "test1";
            const string key2 = "test2";
            const string data = "hello world";

            testContext.Session.Object.SetString(key1, data);
            testContext.Session.Object.SetString(key2, data);
            testContext.Session.Object.Clear();

            Assert.That(testContext.Session.Object.Keys.Count, Is.EqualTo(0));
        }
    }
}