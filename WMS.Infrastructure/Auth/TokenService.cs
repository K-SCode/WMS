using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Entities.Auth;
using WMS.Infrastructure.Data.Identity;

namespace WMS.Infrastructure.Auth
{
    public class TokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<ApplicationUser> userManager) : ITokenService
    {
        private readonly JwtSettings _settings = jwtSettings.Value;

        public async Task<AuthToken> CreateTokenAsync(int userId)
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);
            var user = await userManager.FindByIdAsync(userId.ToString())
                ?? throw new InvalidDataException($"Id:{userId} user not founded");

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(roles.Select(r => new Claim("role", r)));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                SigningCredentials = credentials
            };
            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
            var token = handler.CreateToken(descriptor);

            return new AuthToken(token, expiresAt);
        }
    }
}
