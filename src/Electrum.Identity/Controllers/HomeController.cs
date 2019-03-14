using Microsoft.AspNetCore.Mvc;

namespace Electrum.Identity.Controllers
{
    [Route("")]
    public class HomeController: ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Electrum Identity Service");
    }
}