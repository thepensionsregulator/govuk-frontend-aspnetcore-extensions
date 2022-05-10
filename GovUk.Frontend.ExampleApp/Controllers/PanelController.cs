using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class PanelController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
