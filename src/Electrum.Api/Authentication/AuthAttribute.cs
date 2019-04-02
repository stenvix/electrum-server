using Microsoft.AspNetCore.Authorization;

namespace Electrum.Api.Authentication
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public AuthAttribute(string scheme, string policy = "") : base(policy)
        {
            AuthenticationSchemes = scheme;
        }
    }
}
