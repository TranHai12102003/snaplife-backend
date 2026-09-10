using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SL.Domain.Common.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SL.Services.Helpers
{
    public static class GenerateToken
    {
        public static string GenerateTokenJWT(IConfiguration configuration, string userId, string? email, string? userName, IList<string>? roles = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"] ?? "JWT Login"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(Strings.JwtClaims.Id, userId),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(Strings.JwtClaims.Name, userName ?? string.Empty),
                new Claim(ClaimTypes.Name, userName ?? string.Empty),
                new Claim(ClaimTypes.Email, email ?? string.Empty),
            };

            if (roles != null && roles.Count > 0)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var secretKey = configuration["Jwt:Key"] ?? "c5fcc0a6-e1f4-49cc-a28f-0d2234241176";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"] ?? "SnapLife",
                audience: configuration["Jwt:Audience"] ?? "SnapLifeUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(360),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

