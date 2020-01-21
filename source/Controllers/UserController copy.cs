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

        [HttpPost]
        public async Task<string> POST(User post){

            _context.Users.Add(post);
            await _context.SaveChangesAsync();
            return "User has been successfully added";
        }
        [HttpPut]
        public async Task<string> Edit(User user){

            List<User> UserList = _context.Users.ToList();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "User has been successfully updated";
        }
        [HttpDelete]
        public async Task<string> Delete(User user){

            List<User> PostList = _context.Users.ToList();
            _context.Entry(user).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your user has been deleted";
        }

    }
}
