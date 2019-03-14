using Microsoft.AspNetCore.Mvc;

namespace Electrum.App.Controllers
{
    public class IndexController : ControllerBase
    {
        public IActionResult Index()
        {
            return Content("Working...");
        }
    }
}
