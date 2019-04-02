using Electrum.Common.Mongo;
using Electrum.Identity.Domain;
using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoRepository<User> _repository;

        public UserRepository(IMongoRepository<User> repository)
        {
            _repository = repository;
        }

        public Task<User> GetAsync(Guid id) => _repository.GetAsync(id);
        public Task<User> GetAsync(string email) => _repository.GetAsync(i => i.Email == email);
        public Task AddAsync(User user) => _repository.AddAsync(user);
    }
}
