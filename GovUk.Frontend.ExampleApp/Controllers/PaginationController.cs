using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class PaginationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
