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
    [Route("api/[controller]")]  
    [ApiController]  
    public class LoginController : Controller  
    {  
        private IConfiguration _config;
        private ApplicationUser _userContext;
    
        public LoginController(IConfiguration config, ApplicationUser userContext)  
        {  
            _config = config;
            _userContext = userContext;
        }  
        [AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]UserModel login)  
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
    
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new { token = tokenString });  
            }  
    
            return response;  
        }  
    
        private string GenerateJSONWebToken(UserModel userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);  

            var claims = new[] {  
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),  
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),  
                new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())  
            };  
    
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],  
                _config["Jwt:Issuer"],  
                claims,  
                expires: DateTime.Now.AddMinutes(120),  
                signingCredentials: credentials);  
    
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
    
        private UserModel AuthenticateUser(UserModel login)  
        {  
            UserModel user = null;  
    
            //Validate the User Credentials  
            //Demo Purpose, I have Passed HardCoded User Information  
            if (login.Username == "Jignesh")  
            {  
                user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com" };  
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
        public async Task<string> Create(UserModel user)
        {
            _userContext.Add(user);
            //user.Password = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]));
            await _userContext.SaveChangesAsync();
            return "new user created";
        }
    }  
}  