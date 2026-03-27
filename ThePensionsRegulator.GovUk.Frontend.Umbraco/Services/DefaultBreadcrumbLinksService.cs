using ThePensionsRegulator.GovUk.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.Services
{
    public class DefaultBreadcrumbLinksService(IDocumentNavigationQueryService _navigationQueryService,
        IPublishedContentStatusFilteringService _publishedStatusFilteringService) : IGovUkBreadcrumbLinksService
    {
        public BreadcrumbViewModel GetLinks(IPublishedContent page)
        {
            BreadcrumbViewModel breadcrumbViewModel = new();
            if (page.Parent<IPublishedContent>(_navigationQueryService, _publishedStatusFilteringService) is null)
            {
                // Return empty breadcrumb so that it doesn't get rendered on the homepage
                return breadcrumbViewModel;
            }
            foreach (var ancestor in page.Ancestors(_navigationQueryService, _publishedStatusFilteringService).OrderBy(x => x.Level))
            {
                breadcrumbViewModel.Links.Add(new BreadcrumbLink { Name = ancestor.Name, Url = ancestor.Url() });
            }

            breadcrumbViewModel.Links.Add(new BreadcrumbLink { Name = page.Name });
            breadcrumbViewModel.CurrentPage = page;
            return breadcrumbViewModel;
        }
    }
}
