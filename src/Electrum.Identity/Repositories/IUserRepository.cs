using Electrum.Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(Guid id);
        Task<User> GetAsync(string email);
        Task AddAsync(User user);
    }
}
