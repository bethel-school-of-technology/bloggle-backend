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

        [HttpGet]
        public ActionResult<IEnumerable<Post>> Get()
        {
            List<Post> PostList = _context.Posts.ToList();
            return PostList;
        }

        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<Post>> GetUserPosts(long userId)
        {
            List<Post> PostList = _context.Posts.Where(o=>o.UserId == userId).OrderByDescending(o=>o.DateCreated).ToList();
            return PostList;
        }

        // GET api/posts
        [HttpGet("single/{postId}")]
        public ActionResult<Post> GetPost(long postId)
        {
            Post post = _context.Posts.First(o=>o.Id == postId);
            return post;
        }

        [HttpGet("feed/{followingString}")] //for testing
        //[HttpGet("feed")] //for launch
        public ActionResult<IEnumerable<Post>> Get(String followingString)
        {
            //10,000 ticks per milisecond
            //long month = 25920000000000; //ticks in a month
            long week = 6048000000000; //ticks in a week
            //may not need to convert to universal time
            DateTime pastTime =  new DateTime(DateTime.Now.ToUniversalTime().Ticks - week*2);
            List<Post> PostList = _context.Posts.Where(o=>o.DateCreated > pastTime).OrderByDescending(o=>o.DateCreated).ToList();
            List<Post> Feed = new List<Post>();
            
            long[] userIds = Array.ConvertAll(followingString.Split(";"), long.Parse);
            foreach(Post post in PostList){
                foreach(long id in userIds){
                    if(post.UserId == id){
                        Feed.Add(post);
                        break;
                    }
                }
                if(Feed.Count<=20){
                    break;
                }
            }
            return Feed;
        }

        [HttpPost]
        public async Task<string> POST(Post post){

            if(post.IsDraft == 1){
                post.DateCreated = null;
            }else{
                User user = _context.Users.First(o=>o.Id == post.UserId);
                user.TotalPosts += 1;
                _context.Entry(user).State = EntityState.Modified;
            }

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return "Record has been successfully added";
        }

        [HttpPut]
        public async Task<string> Edit(Post post){

            if(post.IsDraft == 0){

                Post currentPost = _context.Posts.First(o=>o.Id == post.Id);
                //if the draft state changes add 1 to total posts for the user that posted it
                if (currentPost.IsDraft == 1){
                    User user = _context.Users.First(o=>o.Id == post.UserId);
                    user.TotalPosts += 1;
                    _context.Entry(user).State = EntityState.Modified;
                }
                if(post.DateCreated != null){
                    post.DateModified = DateTime.Now;
                }
            }
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Post has been successfully updated";
        }

        [HttpDelete]
        public async Task<string> Delete(Post post){

            if(post.IsDraft == 0){
                User user = _context.Users.First(o=>o.Id == post.UserId);
                user.TotalPosts -= 1;
                _context.Entry(user).State = EntityState.Modified;
            }

            _context.Entry(post).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Post has been successfully updated";
        }
    }
}
