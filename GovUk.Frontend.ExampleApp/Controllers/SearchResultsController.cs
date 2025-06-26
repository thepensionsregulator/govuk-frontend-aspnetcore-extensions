using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SearchResultsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
