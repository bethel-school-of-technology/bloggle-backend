using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using collaby_backend.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace collaby_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ApplicationUserDb _appUserContext;

        public UserController(ApplicationDbContext context, ApplicationUserDb appUserContext)
        {
            _context = context;
            _appUserContext = appUserContext;
        }

        // GET api/users
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> UserList = _context.Users.ToList();
            return UserList;
        }

        [HttpGet("user/{username}")] //profile page
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult<User> Get(String username)
        {
            User user = _context.Users.First(obj=>obj.UserName == username);
            return user;
        }

        /*[HttpGet("TopRated")]
        public ActionResult<IEnumerable<User>> TopRatedUsers(String username)
        {
            //List<User> UserList = _context.Users.Contains(username).OrderBy(o=>o.TotalPosts).ToList();
            var UserList = _context.Users.Where(b => b.UserName == username).ToList();

            return UserList;
        }*/

        [HttpGet("usernameSearch/{username}")]
        [AllowAnonymous]
        public ActionResult<User> UserNameSearch(String username)
        {
            User user = _context.Users.First(obj=>obj.UserName == username);
            return user;
        }

        [HttpGet("nameSearch/{name}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<User>> NameSearch(String name)
        {
            List<User> UserList;
            String[] fullName;

            if(name.Contains('_')){
                //search by last name if first character is a space
                if(name[0] == '_'){
                    UserList = _context.Users.Where(b => b.LastName == name.Replace("_","")).ToList();
                //otherwise find any matching first and last names
                }else{
                    fullName = name.Split("_");
                    UserList = _context.Users.Where(b => (b.FirstName == fullName[0]) && (b.LastName == fullName[1])).ToList();
                }
            //search by first
            }else{
                UserList = _context.Users.Where(b => b.FirstName == name).ToList();
            }
            if(UserList == null){
                return NotFound();
            }
            return UserList;
        }

        [HttpGet("followList/{followingString}")]
        public ActionResult<IEnumerable<User>> FollowingList([FromBody]String followingString,[FromHeader(Name = "token")]String jwt){

            List<User> followedUsers = new List<User>();
            var decodedJwt = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
            var payload = decodedJwt.Payload;
            
            if(followingString == null){
                //no results found
                return null;
            }

            foreach(String username in followingString.Split(";")){
                User user = _context.Users.First(o=>o.UserName == username);
                //need to confirm if return type is null if First method does not return a user
                if (user != null){
                    followedUsers.Add(user);
                }
            }
            return followedUsers;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<string> Create([FromBody]CreateUser user)
        {
            string[] resultArr = new string[4];

            resultArr[0]=Verify.ValidateEmail(user.Email);
            resultArr[1]=Verify.ValidateName(new string[]{user.FirstName, user.LastName});
            resultArr[2]=Verify.ValidateUserName(user.UserName);
            resultArr[3]=Verify.ValidatePassword(user.Password);

            foreach(string result in resultArr){
                if(result != null){ return result; }
            }

            DateTime currentTime = DateTime.UtcNow;
            string timeString = (currentTime.Ticks/10000).ToString(); //salt string for hashing
            user.Password = Hashing.GenerateSHA256String(user.Password,timeString);

            try{
                AppUser privateInfo = new AppUser{ UserName = user.UserName, Password = user.Password, Email = user.Email, DateCreated = currentTime };
                _appUserContext.Add(privateInfo);
                await _appUserContext.SaveChangesAsync(); //first confirm if email and username are unquie (should be auto handled by database)
            }catch{
                return "Username or Email has already been used";
            }

            User publicInfo = new User{ UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Location = user.Location, Img = user.Img};
            _context.Add(publicInfo);
            await _context.SaveChangesAsync();
            return "new user created";
        }

        [HttpPut]
        public async Task<string> Edit(User user){

            //string[] resultArr = new string[4];

            //resultArr[0]=Verify.ValidateName(new string[]{user.FirstName, user.LastName});
            //resultArr[1]=Verify.ValidateUserName(user.UserName);
            //resultArr[2]=Verify.ValidatePassword(user.Password);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "User has been successfully updated";
        }

        //admin only method
        /*private async Task BandUser(User user){

            user.IsBand = 1;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }*/

        [HttpPut("follow/{username}")]
        public async Task<string> addFollowing(long id, String username){

            User user = _context.Users.First(obj=>obj.Id == id);
            String followingString = user.Followings;

            if(user.UserName==username){
                return "Yeah... not going to let you follow yourself";
            }
            if(followingString == null){
                user.Followings += username;
            }else{
                foreach(String follow in followingString.Split(";")){
                    if(follow == username){
                        return username+" is already being followed";
                    }
                }
                user.Followings += ";"+username;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return "Added "+username+" to your following list";
        }

        [HttpPut("unfollow/{username}")]
        public async Task<string> removeFollowing(long id, String username){

            User user = _context.Users.First(obj=>obj.Id == id);
            String followingString = user.Followings;

            if(user.Followings != null){

                foreach(String follow in followingString.Split(";")){

                    if(follow == username){
                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return "Removed "+username+" from your following list";
                    }
                }
            }
            return "Unable to unfollow "+username+" because you're not currently following them";
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(long id){

            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }
        [HttpPost("Test")]
        public String[] Test(){
            String testString = "adsf";
            String[] testArray = testString.Split(";");
            return testArray;
        }
    }
}
