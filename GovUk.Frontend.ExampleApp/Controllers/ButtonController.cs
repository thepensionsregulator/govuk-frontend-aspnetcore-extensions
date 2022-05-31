using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class ButtonController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
