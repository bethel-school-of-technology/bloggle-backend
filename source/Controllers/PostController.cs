using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collaby_backend.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private ApplicationDbContext _context;

        public PostController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/posts
        [HttpGet]
        public ActionResult<Post> Get(long postId)
        {
            Post post = _context.Posts.First(o=>o.Id == postId);
            return post;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetUserPosts(long userId)
        {
            List<Post> PostList = _context.Posts.Where(o=>o.UserId == userId).OrderByDescending(o=>o.DateCreated).ToList();
            return PostList;
        }

        [HttpPost]
        public async Task<string> POST(Post post){

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return "Record has been successfully added";
        }

        [HttpPut]
        public async Task<string> Edit(Post post){

            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Post has been successfully updated";
        }

        [HttpDelete]
        public async Task<string> Delete(Post post){

            _context.Entry(post).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Post has been successfully updated";
        }
    }
}
