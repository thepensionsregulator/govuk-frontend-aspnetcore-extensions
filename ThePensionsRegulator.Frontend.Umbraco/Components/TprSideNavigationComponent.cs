using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using ThePensionsRegulator.Frontend.Umbraco.Services;
using ThePensionsRegulator.Umbraco;

namespace ThePensionsRegulator.Frontend.Umbraco.Components
{
    [ViewComponent(Name = "TprSideNavigation")]
    public class TprSideNavigationComponent : ViewComponent
    {
        private ITprSideNavigationLinksService _sideNavigationLinksService;
        private IUmbracoPublishedContentAccessor _publishedContext;

        public TprSideNavigationComponent(ITprSideNavigationLinksService sideNavigationLinksService, IUmbracoPublishedContentAccessor publishedContext)
        {
            _sideNavigationLinksService = sideNavigationLinksService;
            _publishedContext = publishedContext;
        }

        public IViewComponentResult Invoke()
        {
            TprSideNavigationViewModel ViewModel = _sideNavigationLinksService.GetLinks(_publishedContext.PublishedContent);
            return View("SideNavigation", ViewModel);
        }
    }
}
