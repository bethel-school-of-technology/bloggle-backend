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
        }

        [HttpPost]  
        public IActionResult Login([FromBody]Login login)  
        {
            IActionResult response = Unauthorized();
            var userInfo = AuthenticateUser(login);

            if(userInfo.IsBand == 1){
                return Ok(new { token = "", response = "You have been band and cannot loggin" });
            }

            if (userInfo.Password != null)
            {
                var tokenString = Jwt.GenerateJSONWebToken(userInfo, _config);
                response = Ok(new { token = "Bearer "+tokenString, response = "" });
                return response;
                
                //response = Ok(new { payload = Jwt.decryptJSONWebToken(tokenString) });
            }

            return Ok(new { token = "", response = "Email or password was typed incorrectly" }); //Email or password was typed incorrectly
        }
    
        private AppUser AuthenticateUser(Login login)
        {
            AppUser user = new AppUser();

            try{
                user = _appUserContext.AppUsers.First(o => o.Email == login.Email); //validate email/user
            }catch{
                user.Password=null;
                return user;
            }

            string timeString = (user.DateCreated.Ticks/10000).ToString();
            string loginHash = Hashing.GenerateSHA256String(login.Password,timeString);

            if(user.Password != loginHash){ //validate password
                user.Password=null;
            }

            return user;
        }

        //[Authorize]
        [HttpGet("confirmToken")]
        public IActionResult CheckToken()
        {
            string token = Request.Headers["Authorization"];
            try{
                if(DateTime.UtcNow < Jwt.GetValidationDate(token))
                    return Ok( new{response = true});
                else{
                    return Ok( new{response = false});//expired session id
                }
            }catch{}

            return Ok( new{response = false});//incorrect for a session Id
        }
    }
}