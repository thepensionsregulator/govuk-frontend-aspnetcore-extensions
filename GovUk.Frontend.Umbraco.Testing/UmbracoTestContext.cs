using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Security.Principal;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Routing;

namespace GovUk.Frontend.Umbraco.Testing
{
    /// <summary>
    /// A mock Umbraco environment with a current page request
    /// </summary>
    public class UmbracoTestContext
    {
        private const string TEMPLATE_NAME = "MockTemplate";

        /// <summary>
        /// HTTP-specific information about this HTTP request.
        /// </summary>
        public Mock<HttpContext> HttpContext { get; private init; } = new();

        /// <summary>
        /// Represents the incoming side of this HTTP request.
        /// </summary>
        public Mock<HttpRequest> Request { get; private init; } = new();

        /// <summary>
        /// The context associated with the controller for this request.
        /// </summary>
        public ControllerContext ControllerContext { get; private init; }

        /// <summary>
        /// Represents an <see cref="IViewEngine"/> that delegates to one of a collection of view engines.
        /// </summary>
        public Mock<ICompositeViewEngine> CompositeViewEngine { get; private init; } = new();

        /// <summary>
        /// Provides access to a TryGetUmbracoContext bool method that will return <c>true</c> if the "current" <see cref="IUmbracoContext"/> is not null. Provides a Clear() method that will clear the current UmbracoContext object. Provides a Set() method that will set the current UmbracoContext object.
        /// </summary>
        public Mock<IUmbracoContextAccessor> UmbracoContextAccessor { get; private init; } = new();

        /// <summary>
        /// Provides access to the current <see cref="IVariationContextAccessor.VariationContext"/>
        /// </summary>
        public Mock<IVariationContextAccessor> VariationContextAccessor { get; private init; } = new();

        /// <summary>
        /// Represents the Umbraco service context, which provides access to all services.
        /// </summary>
        public ServiceContext ServiceContext { get; private init; }

        /// <summary>
        /// Gets the current content item.
        /// </summary>
        public Mock<IPublishedContent> CurrentPage { get; private init; }

        /// <summary>
        /// Gets or sets the object used to manage user session data for this request.
        /// </summary>
        public Mock<ISession> Session { get; private init; } = new();

        /// <summary>
        /// Provides access to Umbraco's cache of current published content.
        /// </summary>
        public Mock<IPublishedContentCache> PublishedContentCache { get; private init; } = new();

        public UmbracoTestContext()
        {
            CurrentPage = UmbracoContentFactory.CreateContent<IPublishedContent>("MockDocumentType");

            SetupHttpContext();

            ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            CompositeViewEngine.Setup(x => x.FindView(ControllerContext, TEMPLATE_NAME, false))
                .Returns(ViewEngineResult.Found(TEMPLATE_NAME, Mock.Of<IView>()));

            var umbracoContextMock = new Mock<IUmbracoContext>();
            umbracoContextMock.Setup(context => context.Content).Returns(PublishedContentCache.Object);
            umbracoContextMock.Setup(context => context.PublishedRequest).Returns(HttpContext.Object.Features.Get<UmbracoRouteValues>()!.PublishedRequest);

            var umbracoContext = umbracoContextMock.Object;
            UmbracoContextAccessor.Setup(x => x.TryGetUmbracoContext(out umbracoContext)).Returns(true);

            ServiceContext = ServiceContext.CreatePartial(
                localizationService: Mock.Of<ILocalizationService>()
            );
        }

        private void SetupHttpContext()
        {
            Request.SetupGet(x => x.Scheme).Returns("https");
            Request.SetupGet(x => x.Host).Returns(new HostString("example.org"));
            Request.SetupGet(x => x.Query).Returns(new QueryCollection());
            Request.SetupGet(x => x.Headers).Returns(new HeaderDictionary());

            HttpContext.SetupGet(x => x.Request).Returns(Request.Object);

            var identity = new Mock<IIdentity>();
            identity.Setup(x => x.IsAuthenticated).Returns(false);
            var user = new GenericPrincipal(identity.Object, Array.Empty<string>());
            HttpContext.Setup(x => x.User).Returns(user);

            // Configure Umbraco to get the currently logged-in member based on the same user
            // by mocking responses to the steps taken internally by MembershipHelper.GetCurrentUser()
            Thread.CurrentPrincipal = user;

            HttpContext.SetupGet(x => x.Session).Returns(Session.Object);

            var publishedRequest = new Mock<IPublishedRequest>();
            publishedRequest.Setup(request => request.PublishedContent).Returns(CurrentPage.Object);

            var features = new FeatureCollection();
            features.Set(new UmbracoRouteValues(publishedRequest.Object, new ControllerActionDescriptor(), TEMPLATE_NAME));
            HttpContext.SetupGet(x => x.Features).Returns(features);

        }
    }
}