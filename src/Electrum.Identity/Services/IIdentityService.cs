using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Services
{
    public interface IIdentityService
    {
        Task SignUpAsync(Guid id, string email, string password, string role);
    }
}
