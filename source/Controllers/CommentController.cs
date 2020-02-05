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

        // GET api/comments/{id}
        [HttpGet("{postId}")]
        public ActionResult<IEnumerable<Comment>> GetAll(long postId)
        {
            List<Comment> CommentList = _context.Comments.Where(o=>o.PostId == postId).OrderByDescending(o=>o.Id).ToList();
            if(CommentList == null){
                return NotFound();
            }
            return CommentList;
        }
        // GET api/comments/single/{id}
        [HttpGet("single/{commentId}")]
        public ActionResult<Comment> GetSingle(long commentId){
            Comment comment = _context.Comments.First(o=>o.Id == commentId);
            return comment;
        }

        [HttpPost]
        public async Task<string> POST(Comment comment){

            if(comment.IsDraft == 1){
                comment.DateCreated = null;
            }else{
                Post post = _context.Posts.First(o=>o.Id == comment.PostId);
                post.TotalComments += 1;
                _context.Entry(post).State = EntityState.Modified;
            }

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return "Comment has been successfully added";
        }
        [HttpPut]
        public async Task<string> Edit(Comment comment){

            Comment currentComment = _context.Comments.First(o=>o.Id == comment.Id);
            if(currentComment == null)
                return "Cannot update a post that hasn't been created";

            if(comment.IsDraft == 0){

                //Post currentPost = _context.Posts.First(o=>o.Id == post.Id);
                //if the draft state changes add 1 to total posts for the user that posted it
                if (currentComment.IsDraft == 1){
                    Post post = _context.Posts.First(o=>o.Id == comment.PostId);
                    post.TotalComments += 1;
                    _context.Entry(post).State = EntityState.Modified;
                    comment.DateCreated = DateTime.UtcNow;
                }
            }

            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Comment has been successfully updated";
        }
        [HttpDelete]
        public async Task<string> Delete(Comment comment){

            if(comment.IsDraft != 1){
                Post post = _context.Posts.First(o=>o.Id == comment.PostId);
                post.TotalComments -= 1;
                _context.Entry(post).State = EntityState.Modified;
            }
            _context.Entry(comment).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your comment has been deleted";
        }

    }
}
