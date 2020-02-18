using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using collaby_backend.Models;
using collaby_backend.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace collaby_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ApplicationUserDb _appUserContext;

        public UserController(ApplicationDbContext context, ApplicationUserDb appUserContext)
        {
            _context = context;
            _appUserContext = appUserContext;
        }

        private int GetUserId(){

            string token = Request.Headers["Authorization"];
            int userId = Int16.Parse(Jwt.decryptJSONWebToken(token)["Id"].ToString());

            return userId;
        }
        private bool isAdmin(){

            string token = Request.Headers["Authorization"];
            string adminId = Jwt.decryptJSONWebToken(token)["IsAdmin"].ToString();
            if(adminId == "1"){
                return true;
            }

            return false;
        }


        // GET api/users
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> UserList = _context.Users.ToList();
            return UserList;
        }

        [HttpGet("profile")] //profile page
        public ActionResult<User> GetProfile()
        {
            User user = _context.Users.First(obj=>obj.Id == GetUserId());
            List<Post> posts = new List<Post>();

            posts = _context.Posts.Where(obj=>obj.UserId == GetUserId() && obj.RatingCount != 0).ToList();
            return user;
            if(posts.Count != 0){
                double? avg = 0;
                foreach(Post post in posts){
                    avg += post.RatingValue / post.RatingCount;
                }
                user.TotalRating = avg;
            }
            return user;
        }

        [HttpGet("profile/{username}")] //profile page
        [AllowAnonymous]
        public ActionResult<User> GetUserProfile(String username)
        {
            User user = _context.Users.First(obj=>obj.UserName == username);
            List<Post> posts = new List<Post>();
            try{
                posts = _context.Posts.Where(obj=>obj.UserId == GetUserId() && obj.RatingCount != 0).ToList();
                double? avg = 0;
                foreach(Post post in posts){
                    avg += post.RatingValue / post.RatingCount;
                }
                user.TotalRating = avg;
            }catch{

            }
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

        [HttpGet("followList")]
        public ActionResult<IEnumerable<User>> FollowingList(){

            List<User> followedUsers = new List<User>();
            string followingString = _context.Users.First(o=>o.Id == GetUserId()).Followings;
            
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
        public async Task<Object> Create([FromBody]CreateUser user)
        {
            string[] resultArr = new string[4];

            resultArr[0]=Verify.ValidateEmail(user.Email);
            resultArr[1]=Verify.ValidateName(new string[]{user.FirstName, user.LastName});
            resultArr[2]=Verify.ValidateUserName(user.UserName);
            resultArr[3]=Verify.ValidatePassword(user.Password);
            
            foreach(string result in resultArr){
                if(result != null){ return Ok(new { response = result}); }
            }

            DateTime currentTime = DateTime.UtcNow;
            string timeString = (currentTime.Ticks/10000).ToString(); //salt string for hashing
            user.Password = Hashing.GenerateSHA256String(user.Password,timeString);

            try{
                AppUser privateInfo = new AppUser{ UserName = user.UserName, Password = user.Password, Email = user.Email, DateCreated = currentTime };
                _appUserContext.Add(privateInfo);
                await _appUserContext.SaveChangesAsync(); //first confirm if email and username are unquie (should be auto handled by database)
            }catch{
                return Ok(new { response = "Username or Email has already been used"});
            }

            User publicInfo = new User{ UserName = user.UserName, FirstName = user.FirstName, LastName = user.LastName, Location = user.Location, Img = user.Img};
            _context.Add(publicInfo);
            await _context.SaveChangesAsync();
            return Ok(new { response = "new user created"});
        }

        [HttpPut]
        public async Task<Object> Edit(User user){

            //string[] resultArr = new string[4];

            //resultArr[0]=Verify.ValidateName(new string[]{user.FirstName, user.LastName});
            //resultArr[1]=Verify.ValidateUserName(user.UserName);
            //resultArr[2]=Verify.ValidatePassword(user.Password);

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { response = "User has been successfully updated"});
        }

        [HttpPut("follow/{username}")]
        public async Task<Object> addFollowing(String username){

            User user = _context.Users.First(obj=>obj.Id == GetUserId());
            String followingString = user.Followings;

            if(user.UserName == username){
                return Ok(new { response = "Yeah... not going to let you follow yourself"});
            }
            if(followingString == null){
                user.Followings += username;
            }else{
                foreach(String follow in followingString.Split(";")){
                    if(follow == username){
                        return Ok(new { response = username+" is already being followed"});
                    }
                }
                user.Followings += ";"+username;
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            
            return Ok(new { response = "Added "+username+" to your following list"});
        }

        [HttpPut("unfollow/{username}")]
        public async Task<Object> removeFollowing(String username){

            User user = _context.Users.First(obj=>obj.Id == GetUserId());
            String followingString = user.Followings;

            if(user.Followings != null){
                List<String> FollowList = followingString.Split(";").ToList();
                foreach(String follow in FollowList){

                    if(follow == username){

                        FollowList.Remove(follow);
                        String newFollowString=null;

                        foreach(String following in FollowList){
                            if(newFollowString == null){
                                newFollowString=following;
                            }else{
                                newFollowString+=";"+following;
                            }
                        }
                        user.Followings = newFollowString;

                        _context.Entry(user).State = EntityState.Modified;
                        await _context.SaveChangesAsync();
                        return Ok(new { response = "Removed "+username+" from your following list"});
                    }
                }
            }
            return Ok(new { response = "Unable to unfollow "+username+" because you're not currently following them"});
        }

        //meant for testing
        [HttpDelete("{id}")]
        public async Task<Object> Delete(long id){

            var user = await _context.Users.FindAsync(id);
            var appUser = await _appUserContext.AppUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            if(isAdmin() == true){
                _appUserContext.AppUsers.Remove(appUser);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return Ok(new { response = "User with the username of "+user.UserName+" and Id of "+user.Id.ToString()+" has been deleted"});
        }

        [HttpPost("Test")]
        public int Test(){
            
            return GetUserId();
        }
    }
}