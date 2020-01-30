using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using collaby_backend.Models;
      
namespace JWTAuthController.Controllers  
{  
    [Route("api/Login")]  
    [ApiController]  
    public class LoginController : Controller  
    {  
        private IConfiguration _config;
        private ApplicationUser _userContext;
        //private String validation = null;
    
        public LoginController(IConfiguration config, ApplicationUser userContext)  
        {  
            _config = config;
            _userContext = userContext;
            //validation = _userContext.User.FindAsync("");
        }  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]AppUser login)  
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                var decodedString = new JwtSecurityTokenHandler().ReadJwtToken(tokenString);
                //response = Ok(new { token = tokenString });
                response = Ok(new { token = decodedString });
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
                new Claim("CreationDate", userInfo.CreationDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //new Claim("Role", userInfo.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                null,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }  
    
        private AppUser AuthenticateUser(AppUser login)
        {
            AppUser user = null;
    
            //Validate the User Credentials
            //Demo Purpose, I have Passed HardCoded User Information
            if (login.UserName == "Jignesh")
            {
                user = new AppUser {UserName = "Jignesh Trivedi", Email = "test.btest@gmail.com"};
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
        [HttpPost("newUser")]
        public async Task<string> Create(AppUser user)
        {
            _userContext.Add(user);
            //user.PasswordHash = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));
            await _userContext.SaveChangesAsync();
            return "new user created";
        }

        [AllowAnonymous]
        [HttpGet("appSettings")]
        public ActionResult <string> Getter()
        {
            return _config["Jwt:Secret"] +" "+ _config["Jwt:Issuer"];
        }
    }
}