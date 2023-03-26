using CMS_API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CMS_API.JWTService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(User account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>
        {
            new(ClaimTypes.Role, account.Role.Name.ToString()),
            new(ClaimTypes.Email, account.Email.ToString()),
            new(ClaimTypes.Name, account.UserId.ToString())
        };
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["JwtToken:NotTokenKeyForSureSourceTrustMeDude"]));

            var credential = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["JwtToken:Issuer"],
                _configuration["JwtToken:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(21),
                signingCredentials: credential);

            return tokenHandler.WriteToken(token);
        }
    }
}
