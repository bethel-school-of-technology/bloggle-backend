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
        public ActionResult<IEnumerable<Comment>> GetAll(long postId)
        {
            List<Comment> CommentList = _context.Comments.Where(o=>o.PostId == postId).ToList();
            return CommentList;
        }
        public ActionResult<Comment> GetSingle(long commentId){
            Comment comment = _context.Comments.First(o=>o.Id == commentId);
            return comment;
        }

        [HttpPost]
        public async Task<string> POST(Comment comment){

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return "Comment has been successfully added";
        }
        [HttpPut]
        public async Task<string> Edit(Comment comment){

            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Comment has been successfully updated";
        }
        [HttpDelete]
        public async Task<string> Delete(Comment comment){

            _context.Entry(comment).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your comment has been deleted";
        }

    }
}
