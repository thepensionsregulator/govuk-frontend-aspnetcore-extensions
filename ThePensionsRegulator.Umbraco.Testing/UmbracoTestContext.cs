using Examine;
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
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Routing;
using di = Umbraco.Cms.Web.Common.DependencyInjection;

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
        /// Provides a fallback strategy for getting <see cref="IPublishedElement"/> values.
        /// </summary>
        public Mock<IPublishedValueFallback> PublishedValueFallback { get; private init; } = new();

        /// <summary>
        /// Provides the published model creation service.
        /// </summary>
        public Mock<IPublishedModelFactory> PublishedModelFactory { get; private init; } = new();

        /// <summary>
        /// Gets the url of published content.
        /// </summary>
        public Mock<IPublishedUrlProvider> PublishedUrlProvider { get; private init; } = new();

        /// <summary>
        /// Provides utilities to handle site domains.
        /// </summary>
        public Mock<ISiteDomainMapper> SiteDomainMapper { get; private init; } = new();

        /// <summary>
        /// Provides access to the Examine search engine.
        /// </summary>
        public Mock<IExamineManager> ExamineManager { get; private init; } = new();

        /// <summary>
        /// Settings for routing Umbraco web requests.
        /// </summary>
        public WebRoutingSettings WebRoutingSettings { get; private init; } = new();

        /// <summary>
        /// Represents the Umbraco service context, which provides access to all services.
        /// </summary>
        public ServiceContext ServiceContext { get; private init; }

        /// <summary>
        /// The service provider used by Umbraco to look up internal services using dependency injection.
        /// </summary>
        public Mock<IServiceProvider> ServiceProvider { get; private init; } = new();

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
        /// Represents a service for handling audit.
        /// </summary>
        public Mock<IAuditService> AuditService { get; private init; } = new();

        /// <summary>
        /// A service for handling lawful data processing requirements.
        /// </summary>
        public Mock<IConsentService> ConsentService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IContent" />.
        /// </summary>
        public Mock<IContentService> ContentService { get; private init; } = new();

        /// <summary>
        /// Provides the <see cref="IContentTypeBaseService" /> corresponding to an <see cref="IContentBase" /> object.
        /// </summary>
        public Mock<IContentTypeBaseServiceProvider> ContentTypeBaseServiceProvider { get; private init; } = new();

        /// <summary>
        /// Manages <see cref="IContentType" /> objects.
        /// </summary>
        public Mock<IContentTypeService> ContentTypeService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IDataType" />.
        /// </summary>
        public Mock<IDataTypeService> DataTypeService { get; private init; } = new();

        /// <summary>
        /// Manages domains assigned to content nodes in a multi-site instance.
        /// </summary>
        public Mock<IDomainService> DomainService { get; private init; } = new();

        /// <summary>
        /// Manage entities.
        /// </summary>
        public Mock<IEntityService> EntityService { get; private init; } = new();

        /// <summary>
        /// Used to store the external login info.
        /// </summary>
        public Mock<IExternalLoginService> ExternalLoginService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IFile" /> objects like Scripts, Stylesheets and Templates.
        /// </summary>
        public Mock<IFileService> FileService { get; private init; } = new();

        /// <summary>
        /// Manages the simplified key/value store.
        /// </summary>
        public Mock<IKeyValueService> KeyValueService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving Languages and Dictionary.
        /// </summary>
        public Mock<ILocalizationService> LocalizationService { get; private init; } = new();

        /// <summary>
        /// The entry point to localize any key in the text storage source for a given culture.
        /// </summary>
        public Mock<ILocalizedTextService> LocalizedTextService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IMacro" />.
        /// </summary>
        public Mock<IMacroService> MacroService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IMedia" />.
        /// </summary>
        public Mock<IMediaService> MediaService { get; private init; } = new();

        /// <summary>
        /// Manages <see cref="IMediaType" /> objects.
        /// </summary>
        public Mock<IMediaTypeService> MediaTypeService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving Umbraco members.
        /// </summary>
        public Mock<IMemberService> MemberService { get; private init; } = new();

        /// <summary>
        /// Manages member groups.
        /// </summary>
        public Mock<IMemberGroupService> MemberGroupService { get; private init; } = new();

        /// <summary>
        /// Manages <see cref="IMemberType" /> objects.
        /// </summary>
        public Mock<IMemberTypeService> MemberTypeService { get; private init; } = new();

        /// <summary>
        /// Manages Umbraco notifications.
        /// </summary>
        public Mock<INotificationService> NotificationService { get; private init; } = new();

        /// <summary>
        /// Manages Umbraco packages.
        /// </summary>
        public Mock<IPackagingService> PackagingService { get; private init; } = new();

        /// <summary>
        /// Manages the rules for controlling public access to Umbraco content.
        /// </summary>
        public Mock<IPublicAccessService> PublicAccessService { get; private init; } = new();

        /// <summary>
        /// Manages redirects for Umbraco content nodes.
        /// </summary>
        public Mock<IRedirectUrlService> RedirectUrlService { get; private init; } = new();

        /// <summary>
        /// Manages relations between Umbraco entities.
        /// </summary>
        public Mock<IRelationService> RelationService { get; private init; } = new();

        /// <summary>
        /// Manages the roles served by Umbraco instances in a distributed deployment.
        /// </summary>
        public Mock<IServerRegistrationService> ServerRegistrationService { get; private init; } = new();

        /// <summary>
        /// Tag service to query for tags in the tags db table. The tags returned are only relevant for published content & saved media or members.
        /// </summary>
        public Mock<ITagService> TagService { get; private init; } = new();

        /// <summary>
        /// Provides easy access to operations involving <see cref="IProfile" /> and eventually Users.
        /// </summary>
        public Mock<IUserService> UserService { get; private init; } = new();

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
                ContentService.Object,
                MediaService.Object,
                ContentTypeService.Object,
                MediaTypeService.Object,
                DataTypeService.Object,
                FileService.Object,
                LocalizationService.Object,
                PackagingService.Object,
                EntityService.Object,
                RelationService.Object,
                MemberGroupService.Object,
                MemberTypeService.Object,
                MemberService.Object,
                UserService.Object,
                TagService.Object,
                NotificationService.Object,
                LocalizedTextService.Object,
                AuditService.Object,
                DomainService.Object,
                MacroService.Object,
                PublicAccessService.Object,
                ExternalLoginService.Object,
                ServerRegistrationService.Object,
                RedirectUrlService.Object,
                ConsentService.Object,
                KeyValueService.Object,
                ContentTypeBaseServiceProvider.Object
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

            ContentTypeService.Setup(x => x.GetAllContentTypeAliases()).Returns(_contentTypes.Keys.ToArray());
            ContentTypeService.Setup(x => x.GetAllContentTypeIds(It.IsAny<string[]>())).Returns<string[]>(aliases => _contentTypes.Where(x => aliases.Contains(x.Value.Object.Alias)).Select(x => x.Value.Object.Id));

            SetupServices();
        }

        private void SetupServices()
        {
            di.StaticServiceProvider.Instance = ServiceProvider.Object;
            HttpContext.Setup(x => x.RequestServices).Returns(ServiceProvider.Object);
            SetupService(AuditService.Object);
            SetupService(CompositeViewEngine.Object);
            SetupService(ConsentService.Object);
            SetupService(ContentService.Object);
            SetupService(ContentTypeBaseServiceProvider.Object);
            SetupService(ContentTypeService.Object);
            SetupService(DataTypeService.Object);
            SetupService(DomainService.Object);
            SetupService(EntityService.Object);
            SetupService(ExternalLoginService.Object);
            SetupService(ExamineManager.Object);
            SetupService(FileService.Object);
            SetupService(KeyValueService.Object);
            SetupService(LocalizationService.Object);
            SetupService(LocalizedTextService.Object);
            SetupService(MacroService.Object);
            SetupService(MediaService.Object);
            SetupService(MediaTypeService.Object);
            SetupService(MemberService.Object);
            SetupService(MemberGroupService.Object);
            SetupService(MemberTypeService.Object);
            SetupService(NotificationService.Object);
            SetupService(PackagingService.Object);
            SetupService(PublicAccessService.Object);
            SetupService(PublishedContentCache.Object);
            SetupService(PublishedModelFactory.Object);
            SetupService(PublishedSnapshotAccessor.Object);
            SetupService(PublishedUrlProvider.Object);
            SetupService(PublishedValueFallback.Object);
            SetupService(RedirectUrlService.Object);
            SetupService(RelationService.Object);
            SetupService(ServerRegistrationService.Object);
            SetupService(SiteDomainMapper.Object);
            SetupService(TagService.Object);
            SetupService(UmbracoContextAccessor.Object);
            SetupService(UserService.Object);
            SetupService(VariationContextAccessor.Object);
            SetupService(Options.Create(WebRoutingSettings));
        }

        private void SetupService<T>(T implementation)
        {
            ServiceProvider.Setup(x => x.GetService(typeof(T))).Returns(implementation);
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
            contentType.Setup(x => x.Id).Returns(_contentTypes.Count + 1);
            contentType.Setup(x => x.Key).Returns(Guid.NewGuid());
            contentType.Setup(x => x.Alias).Returns(contentTypeAlias);

            _contentTypes.Add(contentTypeAlias, contentType);
            ContentTypes = new ReadOnlyDictionary<string, Mock<IPublishedContentType>>(_contentTypes);

            PublishedContentCache.Setup(x => x.GetContentType(contentTypeAlias)).Returns<string>(alias => ContentTypes[alias].Object);

            return this;
        }
    }
}