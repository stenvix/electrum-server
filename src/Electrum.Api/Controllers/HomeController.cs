using Microsoft.AspNetCore.Mvc;

namespace Electrum.Api.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Electrum API Service");
    }
}
