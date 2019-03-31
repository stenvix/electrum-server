using Electrum.Common.Types;
using Electrum.Identity.Context;
using Electrum.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUnitOfWork<IdentityContext> _unitOfWork;
        private readonly IRepository<User> _userRepository;

        public IdentityService(IUnitOfWork<IdentityContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userRepository = _unitOfWork.GetRepository<User>();
        }

        public async Task SignUpAsync(Guid id, string email, string password, string role = Role.User)
        {
            var user = await _userRepository.GetFirstOrDefaultAsync(predicate: i => i.Email == email);
            if (user != null)
            {
                throw new ElectrumException(Codes.EmailInUse, $"Email: '{email}' is already in use."); ;
            }

            if (string.IsNullOrEmpty(role))
            {
                role = Role.User;
            }

            user = new User(id, email, role);
            user.SetPassword(password);
            await _userRepository.InsertAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
