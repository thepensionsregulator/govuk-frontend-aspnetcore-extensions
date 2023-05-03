using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace GovUk.Frontend.Umbraco.Testing.Tests
{
    [TestFixture]
    public class UmbracoTestContextTests
    {
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
    }
}