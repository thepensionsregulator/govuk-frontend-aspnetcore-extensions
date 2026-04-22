using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class BreadcrumbsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("breadcrumbs/more-breadcrumbs")]
        public IActionResult MoreBreadcrumbs()
        {
            return View("MoreBreadcrumbs");
        }

        [Route("breadcrumbs/more-breadcrumbs/yet-more-breadcrumbs")]
        public IActionResult YetMoreBreadcrumbs()
        {
            return View("YetMoreBreadcrumbs");
        }
    }
}
