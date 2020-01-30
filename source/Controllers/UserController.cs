using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace collaby_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ApplicationUser _userContext;

        public UserController(ApplicationDbContext context, ApplicationUser userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        // GET api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> UserList = _context.Users.ToList();
            return UserList;
        }
        [HttpGet("{id}")]
        [ValidateAntiForgeryToken]
        public ActionResult<User> Get(long id)
        {
            User user = _context.Users.First(obj=>obj.Id == id);
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
        public ActionResult<User> UserNameSearch(String username)
        {
            User user = _context.Users.First(obj=>obj.UserName == username);
            return user;
        }

        [HttpGet("nameSearch/{name}")]
        public ActionResult<IEnumerable<User>> NameSearch(String name)
        {
            List<User> UserList;
            String[] fullName;

            if(name.Contains(' ')){
                //search by last name if first character is a space
                if(name[0] == ' '){
                    UserList = _context.Users.Where(b => b.LastName == name).ToList();
                //otherwise find any matching first and last names
                }else{
                    fullName = name.Split();
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
        public ActionResult<IEnumerable<User>> FollowingList(String followingString){

            List<User> followedUsers = new List<User>();
            
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

        [HttpPost]
        public async Task<string> POST(User user){


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User has been successfully added";
        }

        [HttpPut]
        public async Task<string> Edit(User user){

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "User has been successfully updated";
        }

        //admin only method
        private async Task BandUser(User user){

            user.IsBand = 1;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

        }

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
