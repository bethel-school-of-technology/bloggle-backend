using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collaby_backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/user
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            List<User> UserList = _context.Users.ToList();
            return UserList;
        }
        [HttpGet("{id}")]
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
        private async Task UpdatePostCounter(User user){
            
            user.TotalPosts += 1;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        private async Task BandUser(User user){

            user.IsBand = 1;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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

    }
}
