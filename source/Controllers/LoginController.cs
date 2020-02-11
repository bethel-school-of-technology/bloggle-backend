using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using collaby_backend.Models;
using collaby_backend.Helper;
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

        //[AllowAnonymous]  
        [HttpPost]  
        public IActionResult Login([FromBody]Login login)  
        {
            IActionResult response = Unauthorized();
            var userInfo = AuthenticateUser(login);

            if (userInfo != null)
            {
                var tokenString = Jwt.GenerateJSONWebToken(userInfo, _config);
                response = Ok(new { token = "Bearer "+tokenString, user = userInfo });
                return response;
                
                //response = Ok(new { payload = Jwt.decryptJSONWebToken(tokenString) });
            }

            return Ok(new { token = "" }); //Email or password was typed incorrectly
        }
    
        private AppUser AuthenticateUser(Login login)
        {
            AppUser user = new AppUser();

            try{
                user = _appUserContext.AppUsers.First(o => o.Email == login.Email); //validate email/user
            }catch{
                return null;
            }

            string timeString = (user.DateCreated.Ticks/10000).ToString();
            string loginHash = Hashing.GenerateSHA256String(login.Password,timeString);

            if(user.Password != loginHash){ //validate password
                return null; 
            }
            return user;
        }
        
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", "value3", "value4", "value5" };
        }
        public ActionResult<IEnumerable<AppUser>> GetAll()
        {
            List<AppUser> UserList = _appUserContext.AppUsers.ToList();
            return UserList;
        }

        [Authorize]
        [HttpGet("confirmToken")]
        public IActionResult CheckToken()
        {
            return Ok( new{response = "Token is still valid"});
        }
    }
}