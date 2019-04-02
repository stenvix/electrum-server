using Electrum.Common.Authentication;
using Electrum.Common.Types;
using Electrum.Identity.Authentication;
using Electrum.Identity.Domain;
using Electrum.Identity.Repositories;
using System;
using System.Threading.Tasks;

namespace Electrum.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtHandler _jwtHandler;

        public IdentityService(IUserRepository userRepository, IJwtHandler jwtHandler)
        {
            _userRepository = userRepository;
            _jwtHandler = jwtHandler;
        }

        public async Task<JsonWebToken> SignInAsync(string email, string password)
        {
            var user = await _userRepository.GetAsync(email);
            if (user == null || !user.ValidatePassword(password))
            {
                throw new ElectrumException(Codes.InvalidCredentials, "Invalid credentials.");
            }
            //var claims = await _claimsProvider.GetAsync(user.Id);
            var jwt = _jwtHandler.CreateToken(user.Id.ToString("N"), user.Role);
            //await _refreshTokenRepository.AddAsync(refreshToken);
            return jwt;
        }

        public async Task SignUpAsync(Guid id, string email, string password, string role = Role.User)
        {
            var user = await _userRepository.GetAsync(email);
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
            await _userRepository.AddAsync(user);
        }
    }
}
