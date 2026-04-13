using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services.Navigation;

namespace GovUk.Frontend.Umbraco.ExampleApp.Services
{

    public class SideNavigationLinksServiceForExampleApp : UmbracoSideNavigationLinksService
    {
        public SideNavigationLinksServiceForExampleApp(
            IUmbracoPublishedContentAccessor _contentAccessor,
            IDocumentNavigationQueryService _documentNavigationQueryService,
            IPublishedContentStatusFilteringService _publishedStatusFilteringService,
            IPublishedUrlProvider _publishedUrlProvider,
            IPublishedValueFallback _publishedValueFallback) :
            base(_contentAccessor, _documentNavigationQueryService, _publishedStatusFilteringService, _publishedUrlProvider, _publishedValueFallback)
        { }
        protected override IPublishedContent? GetRootContent() => CurrentPage.Root().Children(x => x.Name == "Side navigation").FirstOrDefault() ?? base.GetRootContent();
    }
}
