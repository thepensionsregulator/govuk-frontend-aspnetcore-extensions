using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SummaryListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
