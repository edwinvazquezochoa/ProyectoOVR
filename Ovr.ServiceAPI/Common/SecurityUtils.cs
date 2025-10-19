using Microsoft.IdentityModel.Tokens;
using Ovr.Core.Infrastructures.Configs;
using Ovr.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoNetPos.Ovr.ApiServices.Common
{
    public class SecurityUtils
    {
        private readonly string _jwtKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly SigningCredentials _signingCredentials;

        public SecurityUtils(IConfiguration configuration)
        {
            _jwtKey = ObtenerValores.JwtSettingKey()
                      ?? throw new InvalidOperationException("JWT key is missing.");

            _issuer = ObtenerValores.JwtIssuer();
            _audience = ObtenerValores.JwtAudience();

            var keyBytes = Encoding.UTF8.GetBytes(_jwtKey);
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);
        }

        public string GenerateJwtToken(UserInfo userInfo)
        {
            var claims = new[]
            {
                new Claim("UserId", userInfo.UserId.ToString()),
                new Claim("PersonId", userInfo.PersonId.ToString()),
                new Claim("Username", userInfo.UserName),
                new Claim("ShortName", userInfo.ShortName ?? string.Empty),
                new Claim("FullName", userInfo.FullName ?? string.Empty),
                new Claim("Email", userInfo.Email ?? string.Empty),
                new Claim("RoleName", userInfo.RoleName ?? string.Empty),
                new Claim("RoleId", userInfo.RoleId.ToString()),
                new Claim("BranchId", userInfo.BranchId.ToString()),
                new Claim("BrancheName", userInfo.BrancheName ?? string.Empty),
                new Claim("IsActive", userInfo.IsActive.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = _signingCredentials,
                Issuer = _issuer,
                Audience = _audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
