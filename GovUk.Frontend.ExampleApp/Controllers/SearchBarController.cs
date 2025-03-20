using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class SearchBarController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
