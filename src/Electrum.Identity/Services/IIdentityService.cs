using Electrum.Common.Authentication;
using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Services
{
    public interface IIdentityService
    {
        Task<JsonWebToken> SignInAsync(string email, string password);
        Task SignUpAsync(Guid id, string email, string password, string role);
    }
}
