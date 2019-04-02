using Electrum.Common.Authentication;
using Electrum.Common.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Electrum.Identity.Authentication
{
    public class JwtHandler : IJwtHandler
    {
        private readonly IOptionsMonitor<JwtOptions> _options;

        public JwtHandler(IOptionsMonitor<JwtOptions> options)
        {
            _options = options;
        }

        public JsonWebToken CreateToken(string userId, string role = null, IDictionary<string, string> claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User id claim can not be empty.", nameof(userId));
            }

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };
            if (!string.IsNullOrWhiteSpace(role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray()
                               ?? Array.Empty<Claim>();
            jwtClaims.AddRange(customClaims);
            var options = _options.CurrentValue;
            var expires = now.AddMinutes(options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: GetSigningCredentials(options.SecretKey)
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                Expires = expires.ToTimestamp(),
                Id = userId,
                Role = role ?? string.Empty,
                Claims = customClaims.ToDictionary(c => c.Type, c => c.Value)
            };
        }

        private SigningCredentials GetSigningCredentials(string secretKey)
        {
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            return new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
