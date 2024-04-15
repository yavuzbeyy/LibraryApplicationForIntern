using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.SharedLibrary
{
    public interface ITokenCreator
    {
        string GenerateToken(string username, List<int> roles);
    }

    public class TokenCreator : ITokenCreator
    {
        private readonly IConfiguration _configuration;

        public TokenCreator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username, List<int> roles)
        {
            string secret = _configuration.GetValue<string>("AppSettings:Secret");
            byte[] key = Encoding.UTF8.GetBytes(secret);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("username", username));
            claims.Add(new Claim(ClaimTypes.Name, username));
          //claims.Add(new Claim("UserId", userid.ToString())); // userid'yi ekleyin

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                claims.Add(new Claim("roles", role.ToString()));
            }

            JwtSecurityToken securityToken =
                new JwtSecurityToken(
                    signingCredentials: credentials,
                    claims: claims,
                    expires: DateTime.Now.AddDays(7));

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }


}
