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
    [Route("api/ratings")]
    [ApiController]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId(){

            string token = Request.Headers["Authorization"];
            int userId = Int16.Parse(Jwt.decryptJSONWebToken(token)["Id"].ToString());

            return userId;
        }
        private bool isAdmin(){

            string token = Request.Headers["Authorization"];
            string adminId = Jwt.decryptJSONWebToken(token)["IsAdmin"].ToString();
            if(adminId == "1"){
                return true;
            }

            return false;
        }

        // GET api/ratings/all/{postId}
        [AllowAnonymous]
        [HttpGet("all/{postId}")]
        public ActionResult <IEnumerable<Rating>> GetAll(long postId)
        {
            List<Rating> ratings = new List<Rating>();

            try{
                Post post = _context.Posts.First(o=>o.Id == postId);
                ratings = _context.Ratings.Where(o=>o.PostId == postId).ToList();
            }catch{
                return null;
            }
            return ratings;
        }

        [HttpGet("rating/{postId}")]
        public ActionResult<Rating> Get(long postId)
        {
            Rating rating;
            
            try{
                Post post = _context.Posts.First(o=>o.Id == postId);
                rating = _context.Ratings.First(o=>o.UserId == GetUserId());
            }catch{
                return null;
            }
            
            if(rating == null){
                return null;
            }

            return rating;
        }

        [HttpPost]//post and put method
        public async Task<Object> POST([FromBody]Rating rating){

            Post post =_context.Posts.First(o=>o.Id == rating.PostId);
            rating.UserId = GetUserId();

            if(_context.Ratings.First(o=>o.UserId == rating.UserId) != null){
                //if rating for the post already exisits, edit it's current value
                post.RatingValue += rating.Value - _context.Ratings.First(o=>o.Id == rating.Id).Value;
                _context.Entry(post).State = EntityState.Modified;
                _context.Entry(rating).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { response = "Rating has been successfully updated"});
            }

            post.RatingCount += 1;
            post.RatingValue += rating.Value;
            _context.Ratings.Add(rating);
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { response = "Rating has been successfully added"});
        }
    }
}
