using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class AccordionController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
