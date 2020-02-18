using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using collaby_backend.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace collaby_backend.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId(){

            string token = Request.Headers["Authorization"];
            int userId = Int16.Parse(Jwt.decryptJSONWebToken(token)["Id"].ToString());

            return userId;
        }
        private string GetUserName(){

            string token = Request.Headers["Authorization"];
            string username = Jwt.decryptJSONWebToken(token)["sub"].ToString();

            return username;
        }

        // GET api/comments/{id}
        [HttpGet("{postId}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult<Comment> GetSingle(long commentId){

            Comment comment = _context.Comments.First(o=>o.Id == commentId);
            return comment;
        }

        [HttpPost]
        public async Task<Object> POST(Comment comment){

            Post post = _context.Posts.First(o=>o.Id == comment.PostId);
            if(post.UserId != GetUserId()){
                return StatusCode(401);
            }
            comment.UserId = post.UserId;

            if(comment.IsDraft == 1){
                comment.DateCreated = null;
            }else{
                post.TotalComments += 1;
                _context.Entry(post).State = EntityState.Modified;
            }
            comment.Message = GetUserName()+ ";" +comment.Message;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return Ok(new { response = "Comment has been successfully added"});
        }

        [HttpPut]
        public async Task<Object> Edit(Comment comment){

            Comment currentComment = _context.Comments.First(o=>o.Id == comment.Id);

            if(currentComment.UserId != GetUserId()){
                return StatusCode(401);
            }

            if(currentComment == null)
                return Ok(new { response = "Cannot update a comment that hasn't been created"});

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
            comment.Message = GetUserName()+ ";" + comment.Message;
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { response = "Comment has been successfully updated"});
        }
        [HttpDelete]
        public async Task<Object> Delete(Comment comment){

            if(comment.IsDraft != 1){
                Post post = _context.Posts.First(o=>o.Id == comment.PostId);
                post.TotalComments -= 1;
                _context.Entry(post).State = EntityState.Modified;
            }
            _context.Entry(comment).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(new { response = "Your comment has been deleted"});
        }

    }
}
