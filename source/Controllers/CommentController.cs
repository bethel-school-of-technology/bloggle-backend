using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collaby_backend.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/comments
        [HttpGet]
        public ActionResult<IEnumerable<Comment>> Get()
        {
            List<Comment> CommentList = _context.Comments.ToList();
            return CommentList;
        }

        [HttpPost]
        public async Task<string> POST(Comment report){

            _context.Comments.Add(report);
            await _context.SaveChangesAsync();
            return "Comment has been successfully added";
        }
        [HttpPut]
        public async Task<string> Edit(Comment comment){

            List<Comment> CommentList = _context.Comments.ToList();
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Comment has been successfully updated";
        }
        [HttpDelete]
        public async Task<string> Delete(Comment comment){

            List<Comment> PostList = _context.Comments.ToList();
            _context.Entry(comment).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your comment has been deleted";
        }

    }
}
