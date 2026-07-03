using GovUk.Frontend.Umbraco.Models;
using GovUk.Frontend.Umbraco.Services;
using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Umbraco;

namespace GovUk.Frontend.Umbraco.Components
{
    [ViewComponent(Name = "GovUkBreadcrumb")]
    public class GovUkBreadcrumbComponent : ViewComponent
    {
        private BreadcrumbViewModel? ViewModel;
        private readonly IGovUkAsyncBreadcrumbLinksService? _asyncBreadcrumbLinksService;
        private readonly IGovUkBreadcrumbLinksService? _breadcrumbLinksService;
        private readonly IUmbracoPublishedContentAccessor _publishedContext;

        public GovUkBreadcrumbComponent(
            IUmbracoPublishedContentAccessor publishedContext,
            IGovUkAsyncBreadcrumbLinksService? asyncBreadcrumbLinksService = null,
            IGovUkBreadcrumbLinksService? breadcrumbLinksService = null)
        {
            if (publishedContext is null)
            {
                throw new ArgumentNullException(nameof(publishedContext), "Published context is not initialised.");
            }

            if (asyncBreadcrumbLinksService is null && breadcrumbLinksService is null)
            {
                throw new ArgumentException("At least one breadcrumb links service must be provided.");
            }

            _asyncBreadcrumbLinksService = asyncBreadcrumbLinksService;
            _breadcrumbLinksService = breadcrumbLinksService;
            _publishedContext = publishedContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (_asyncBreadcrumbLinksService is not null)
            {
                ViewModel = await _asyncBreadcrumbLinksService.GetLinks(_publishedContext.PublishedContent);
            }
            else if (_breadcrumbLinksService is not null)
            {
                ViewModel = _breadcrumbLinksService.GetLinks(_publishedContext.PublishedContent);
            }

            return View("Breadcrumb", ViewModel);
        }
    }
}
