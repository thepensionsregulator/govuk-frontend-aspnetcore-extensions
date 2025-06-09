using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class TimelineController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
