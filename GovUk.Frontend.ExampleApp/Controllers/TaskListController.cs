using Microsoft.AspNetCore.Mvc;

namespace GovUk.Frontend.ExampleApp.Controllers
{
    public class TaskListController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
