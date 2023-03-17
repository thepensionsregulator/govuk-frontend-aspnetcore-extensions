using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SummaryCardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
