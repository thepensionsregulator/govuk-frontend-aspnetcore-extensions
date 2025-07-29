using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Umbraco.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Frontend.Umbraco.Components
{
    [ViewComponent(Name = "TprSideNavigation")]
    public class TprSideNavigationComponent : ViewComponent
    {
        public IViewComponentResult Invoke(IPublishedContent currentPage, IPublishedContent rootAncestor, string[] blacklistedUrls)
        {
            TprSideNavigationViewModel ViewModel = new() { CurrentPage = currentPage, RootAncestor = rootAncestor, BlacklistedUrls = blacklistedUrls };
            return View("SideNavigation", ViewModel);
        }
    }
}
