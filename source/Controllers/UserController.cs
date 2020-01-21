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

        [HttpGet]
        public ActionResult<IEnumerable<User>> TopRatedUsers(String username)
        {
            //List<User> UserList = _context.Users.Contains(username).OrderBy(o=>o.TotalPosts).ToList();
            var UserList = _context.Users.Where(b => b.UserName == username).ToList();

            return UserList;
        }

        [HttpGet]
        public ActionResult<User> UserNameSearch(String username)
        {
            User user = _context.Users.First(obj=>obj.UserName == username);
            return user;
        }
        public ActionResult<IEnumerable<User>> NameSearch(String name)
        {
            List<User> UserList = _context.Users.OrderBy(o=>o.TotalPosts).ToList();
            return UserList;
        }

        [HttpPost]
        public async Task<string> POST(User post){

            _context.Users.Add(post);
            await _context.SaveChangesAsync();
            return "User has been successfully added";
        }

        [HttpPut]
        public async Task<string> Edit(User user){

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "User has been successfully updated";
        }

        [HttpDelete]
        public async Task<string> Delete(User user){

            _context.Entry(user).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your user has been deleted";
        }

    }
}
