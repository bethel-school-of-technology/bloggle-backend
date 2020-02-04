using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collaby_backend.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        private ApplicationDbContext _context;

        public RatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/posts
        [HttpGet]
        public ActionResult<Rating> Get(Rating userRating)
        {
            Rating rating = _context.Ratings.First(o=>o.PostId == userRating.PostId && o.UserId == userRating.UserId);
            return rating;
        }

        [HttpPost]
        public async Task<string> POST([FromBody]Rating rating){

            _context.Ratings.Add(rating);
            Post post =_context.Posts.First(o=>o.Id == rating.PostId);
            post.RatingCount += 1;
            post.RatingValue += rating.Value;
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return "Rating has been successfully added";
        }
        
        [HttpPut]
        public async Task<string> Edit([FromBody]Rating rating){

            Post post =_context.Posts.First(o=>o.Id == rating.PostId);
            post.RatingValue += rating.Value - _context.Ratings.First(o=>o.Id == rating.Id).Value;

            _context.Entry(post).State = EntityState.Modified;
            _context.Entry(rating).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Rating has been successfully updated";
        }

    }
}
