using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Security.Principal;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Routing;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// A mock Umbraco environment with a current page request
    /// </summary>
    public class UmbracoTestContext
    {
        private const string TEMPLATE_NAME = "MockTemplate";
        private ClaimsPrincipal _currentPrincipal;
        private DistributedSession _sessionState = new(new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())), Guid.NewGuid().ToString(), TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), () => true, Mock.Of<ILoggerFactory>(), true);

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
        /// The Umbraco context returned by <see cref="UmbracoContextAccessor"/>. 
        /// </summary>
        public Mock<IUmbracoContext> UmbracoContext { get; private init; } = new();

        /// <summary>
        /// Provides access to a TryGetUmbracoContext bool method that will return <c>true</c> if the "current" <see cref="IUmbracoContext"/> is not null. Provides a Clear() method that will clear the current UmbracoContext object. Provides a Set() method that will set the current UmbracoContext object.
        /// </summary>
        public Mock<IUmbracoContextAccessor> UmbracoContextAccessor { get; private init; } = new();

        /// <summary>
        /// Provides access to a TryGetPublishedSnapshot bool method that will return true if the "current" <see cref="IPublishedSnapshot" /> is not null.
        /// </summary>
        public Mock<IPublishedSnapshotAccessor> PublishedSnapshotAccessor { get; private init; } = new();

        /// <summary>
        /// A published snapshot is a point-in-time capture of the current state of everything that is "published".
        /// </summary>
        public Mock<IPublishedSnapshot> PublishedSnapshot { get; private init; } = new();

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
        /// Gets the object used to manage user session data for this request.
        /// </summary>
        public Mock<ISession> Session { get; private init; } = new();

        /// <summary>
        /// The request which is the result of Umbraco routing.
        /// </summary>
        public Mock<IPublishedRequest> PublishedRequest { get; private init; } = new();

        /// <summary>
        /// Provides access to Umbraco's cache of current published content.
        /// </summary>
        public Mock<IPublishedContentCache> PublishedContentCache { get; private init; } = new();

        private readonly Dictionary<string, Mock<IPublishedContentType>> _contentTypes = new();

        /// <summary>
        /// Each content type represents an <see cref="IPublishedElement" /> type. Use <see cref="SetupContentType(string)" /> to add a content type.
        /// </summary>
        public IReadOnlyDictionary<string, Mock<IPublishedContentType>> ContentTypes { get; private set; } =
            new ReadOnlyDictionary<string, Mock<IPublishedContentType>>(new Dictionary<string, Mock<IPublishedContentType>>());

        /// <summary>
        /// The identity associated with the default value of <see cref="CurrentPrincipal"/>.
        /// </summary>
        public Mock<IIdentity> CurrentIdentity { get; private init; } = new();

        /// <summary>
        /// The current user.
        /// </summary>
        public ClaimsPrincipal CurrentPrincipal
        {
            get
            {
                return _currentPrincipal;
            }
            set
            {
                _currentPrincipal = value;
                HttpContext.Setup(x => x.User).Returns(_currentPrincipal);

                // Configure Umbraco to get the currently logged-in member based on the same user
                // by mocking responses to the steps taken internally by MembershipHelper.GetCurrentUser()
                Thread.CurrentPrincipal = _currentPrincipal;
            }
        }

        /// <summary>
        /// Provides easy access to operations involving Languages and Dictionary.
        /// </summary>
        public Mock<ILocalizationService> LocalizationService { get; private init; } = new();

        /// <summary>
        /// The view returned by the <see cref="CompositeViewEngine"/>.
        /// </summary>
        public Mock<IView> View { get; private init; } = new();


        // Disable 'Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.'
        // so that we can use the CurrentPrincipal setter to assign _currentPrincipal.
#pragma warning disable CS8618
        public UmbracoTestContext()
        {
#pragma warning restore CS8618
            CurrentPage = UmbracoContentFactory.CreateContent<IPublishedContent>("MockDocumentType");

            SetupHttpContext();

            ControllerContext = new ControllerContext
            {
                HttpContext = HttpContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new ControllerActionDescriptor()
            };

            CompositeViewEngine.Setup(x => x.FindView(ControllerContext, TEMPLATE_NAME, false))
                .Returns(ViewEngineResult.Found(TEMPLATE_NAME, View.Object));

            UmbracoContext.SetupGet(context => context.Content).Returns(PublishedContentCache.Object);
            UmbracoContext.SetupGet(context => context.PublishedRequest).Returns(HttpContext.Object.Features.Get<UmbracoRouteValues>()!.PublishedRequest);

            var umbracoContextAccessorResult = UmbracoContext.Object;
            UmbracoContextAccessor.Setup(x => x.TryGetUmbracoContext(out umbracoContextAccessorResult)).Returns(true);

            ServiceContext = ServiceContext.CreatePartial(
                localizationService: LocalizationService.Object
            );

            PublishedSnapshot.Setup(x => x.Content).Returns(PublishedContentCache.Object);
            PublishedSnapshotAccessor.Setup(x => x.TryGetPublishedSnapshot(out It.Ref<IPublishedSnapshot>.IsAny!))
                .Callback(new TryGetPublishedSnapshotCallback((out IPublishedSnapshot snapshot) =>
                {
                    snapshot = PublishedSnapshot.Object;
                }))
                .Returns(true);

            CurrentIdentity.SetupGet(x => x.IsAuthenticated).Returns(false);
            CurrentPrincipal = new GenericPrincipal(CurrentIdentity.Object, Array.Empty<string>());
        }

        delegate void TryGetPublishedSnapshotCallback(out IPublishedSnapshot snapshot);
        delegate void SessionTryGetValueCallback(string key, out byte[]? value);
        delegate bool SessionTryGetValueReturns(string key, out byte[]? value);

        private void SetupHttpContext()
        {
            Request.SetupGet(x => x.Scheme).Returns("https");
            Request.SetupGet(x => x.Host).Returns(new HostString("example.org"));
            Request.SetupGet(x => x.Query).Returns(new QueryCollection());
            Request.SetupGet(x => x.Headers).Returns(new HeaderDictionary());
            HttpContext.SetupGet(x => x.Request).Returns(Request.Object);

            // Setup session as a mock which is pre-configured to store and retrieve values from a fake session state
            HttpContext.SetupGet(x => x.Session).Returns(Session.Object);

            Session.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                .Callback<string, byte[]>((key, value) => _sessionState.Set(key, value));
            Session.Setup(x => x.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]?>.IsAny))
                .Callback(new SessionTryGetValueCallback((string key, out byte[]? value) =>
                {
                    _sessionState.TryGetValue(key, out value);
                }))
                .Returns(new SessionTryGetValueReturns((string key, out byte[]? value) => _sessionState.TryGetValue(key, out value)));
            Session.Setup(x => x.Remove(It.IsAny<string>())).Callback<string>(key => _sessionState.Remove(key));
            Session.Setup(x => x.Clear()).Callback(() => _sessionState.Clear());
            Session.Setup(x => x.Keys).Returns(_sessionState.Keys);

            PublishedRequest.SetupGet(request => request.PublishedContent).Returns(CurrentPage.Object);

            var features = new FeatureCollection();
            features.Set(new UmbracoRouteValues(PublishedRequest.Object, new ControllerActionDescriptor(), TEMPLATE_NAME));
            HttpContext.SetupGet(x => x.Features).Returns(features);
        }

        /// <summary>
        /// Adds a new content type to <see cref="ContentTypes"/> and other mocks within the <see cref="UmbracoTestContext"/>.
        /// </summary>
        /// <param name="contentTypeAlias">The alias of the content type.</param>
        /// <returns>The updated test context.</returns>
        public UmbracoTestContext SetupContentType(string contentTypeAlias)
        {
            var contentType = new Mock<IPublishedContentType>();
            contentType.Setup(x => x.Alias).Returns(contentTypeAlias);
            PublishedContentCache.Setup(x => x.GetContentType(contentTypeAlias)).Returns(contentType.Object);

            ContentTypes = new ReadOnlyDictionary<string, Mock<IPublishedContentType>>(_contentTypes);

            return this;
        }
    }
}