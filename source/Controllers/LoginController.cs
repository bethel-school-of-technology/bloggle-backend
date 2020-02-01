using System;
using System.Linq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using collaby_backend.Models;
using collaby_backend.encrpyt;
using Microsoft.EntityFrameworkCore;

      
namespace collaby_backend.Controllers  
{  
    [Route("api/Login")]  
    [ApiController]  
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private ApplicationDbContext _userContext;
        private ApplicationUserDb _appUserContext;
        //private String validation = null;
    
        public LoginController(IConfiguration config, ApplicationDbContext userContext, ApplicationUserDb appUserContext)  
        {  
            _config = config;
            _userContext = userContext;
            _appUserContext = appUserContext;
            //validation = _userContext.User.FindAsync("");
        }

        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]Login login)  
        {
            IActionResult response = Unauthorized();
            var userInfo = AuthenticateUser(login);

            if (userInfo != null)
            {
                var tokenString = GenerateJSONWebToken(userInfo);
                response = Ok(new { token = tokenString });
                //var decodedString = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
                //response = Ok(new { decodedString.Payload });
            }
            return response;
        }
    
        private string GenerateJSONWebToken(AppUser userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("IsAdmin", userInfo.IsAdmin.ToString()),
                new Claim("DateCreated", userInfo.DateCreated.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                notBefore: DateTime.UtcNow,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }  
    
        private AppUser AuthenticateUser(Login login)
        {
            AppUser user = _appUserContext.AppUsers.First(o => o.Email == login.Email);
            string timeString = (user.DateCreated.Ticks/10000).ToString();
            string loginHash = hashing.GenerateSHA256String(login.Password,timeString);

            if(user.Password != loginHash){ 
                return null; 
            }
            return user;
        }
        
        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5" };
        }

        [AllowAnonymous]
        [HttpGet("appSettings")]
        public ActionResult <string> Getter()
        {
            return _config["Jwt:Secret"] +" "+ _config["Jwt:Issuer"];
        }
    }
}