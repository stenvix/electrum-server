using Electrum.Common;
using Electrum.Identity.Messages.Commands;
using Electrum.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Electrum.Identity.Controllers
{
    [Route("")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUp command)
        {
            command.BindId(c => c.Id);
            await _identityService.SignUpAsync(command.Id, command.Email, command.Password, command.Role);
            return NoContent();
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignIn command)
            => Ok(await _identityService.SignInAsync(command.Email, command.Password));
    }
}
