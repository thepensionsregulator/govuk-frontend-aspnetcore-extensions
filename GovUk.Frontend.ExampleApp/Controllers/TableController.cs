using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class TableController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
