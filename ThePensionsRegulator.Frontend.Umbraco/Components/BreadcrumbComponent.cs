using Microsoft.AspNetCore.Mvc;
using ThePensionsRegulator.Frontend.Umbraco.Models;

namespace ThePensionsRegulator.Frontend.Umbraco.Components
{
    [ViewComponent(Name = "Breadcrumb")]
    public class BreadcrumbComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            string[] BreadcrumbNamingPriority = ["PageTitle", "BrowserTitle", "PageContent>govukPageHeading>text"];
            var model = new TPRBreadcrumbModel() { PriorityArray = BreadcrumbNamingPriority };
            return View("Breadcrumb", model);
        }
    }
}
