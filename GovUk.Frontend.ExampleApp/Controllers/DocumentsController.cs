using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class DocumentsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
