using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Components
{
    [ViewComponent(Name = "TprSideNavigation")]
    public class TprSideNavigationComponent : ViewComponent
    {
        private ITprSideNavigationLinksService _sideNavigationLinksService;

        public TprSideNavigationComponent(ITprSideNavigationLinksService sideNavigationLinksService)
        {
            _sideNavigationLinksService = sideNavigationLinksService;
        }

        public IViewComponentResult Invoke()
        {
            TprSideNavigationViewModel? viewModel = _sideNavigationLinksService.GetLinks();
            return viewModel is null ? Content(string.Empty) : View("SideNavigation", viewModel);
        }
    }
}
