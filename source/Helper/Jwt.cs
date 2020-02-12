using System;
using System.Text;
using System.Security.Claims;
using collaby_backend.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;


namespace collaby_backend.Helper{

    class Jwt{

        public static string GenerateJSONWebToken(AppUser userInfo, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("Id", userInfo.Id.ToString()),
                new Claim("IsAdmin", userInfo.IsAdmin.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.UtcNow.AddMinutes(120),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static JwtPayload decryptJSONWebToken(string tokenString){
            var token = tokenString.Split(" ")[1]; //remove 'Bearer ' from string
            var decodedString = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return decodedString.Payload;
        }
    }
}