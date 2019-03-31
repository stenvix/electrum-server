using Electrum.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Electrum.Api.Controllers
{
    [Route("identity")]
    [ApiController]
    public class IdentityController
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
    }
}
