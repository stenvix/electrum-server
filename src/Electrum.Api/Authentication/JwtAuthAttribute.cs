using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Electrum.Api.Authentication
{
    public class JwtAuthAttribute : AuthAttribute
    {
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}
