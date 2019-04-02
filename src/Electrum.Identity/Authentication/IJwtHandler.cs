using Electrum.Common.Authentication;
using System.Collections.Generic;

namespace Electrum.Identity.Authentication
{
    public interface IJwtHandler
    {
        JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null);
    }
}
