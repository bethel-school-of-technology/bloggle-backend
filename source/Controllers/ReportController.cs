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
    [Route("api/reports")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private ApplicationDbContext _context;
        private ApplicationUserDb _appContext;

        public ReportController(ApplicationDbContext context, ApplicationUserDb appContext)
        {
            _context = context;
            _appContext = appContext;
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

        // GET api/reports
        [HttpGet]
        public ActionResult<IEnumerable<Report>> GetAll()
        {
            if(!isAdmin()){
                return StatusCode(401);
            }
            List<Report> ReportList = _context.Reports.OrderByDescending(o=>o.DateCreated).ToList();
            return ReportList;
        }

        //GET api/reports/report/id
        [HttpGet("report/{reportId}")]
        public ActionResult<Report> Get(long reportId)
        {
            if(!isAdmin()){
                return StatusCode(401);
            }
            Report report = _context.Reports.First(o=>o.Id == reportId);
            return report;
        }

        //GET api/reports/post/postId
        [HttpGet("post/{postId}")]
        public ActionResult<IEnumerable<Report>> GetPostReports(long postId)
        {
            if(isAdmin() == false){
               return StatusCode(401);
            }
            List<Report> ReportList = _context.Reports.Where(o=>o.PostId == postId).OrderByDescending(o=>o.DateCreated).ToList();
            return ReportList;
        }

        [HttpPost]
        public async Task<Object> POST([FromBody]Report report){

            report.UserId = GetUserId();

            if(_context.Users.First(o=>o.Id == report.UserId) == null){
                return Ok(new { response = "You have already reported this post"});
            }
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();
            return Ok(new { response = "Report has been successfully added"});
        }

        [HttpDelete("{id}")]
        public async Task<Object> Delete([FromRoute]long id){

            if(!isAdmin()){
                return StatusCode(401);
            }
            Report report = new Report();

            try{
                report = _context.Reports.First(o=>o.Id == id);
            }catch{
                return Ok(new { response = "Invalid report id"});
            }
            _context.Reports.Remove(report).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return Ok(new { response = "Report has been deleted"});
        }
        

        [HttpPut("band/{username}")]
        public async Task<Object> BandUser(String username){

            if(!isAdmin()){
                return StatusCode(401);
            }
            AppUser user = _appContext.AppUsers.First(o=>o.UserName == username);
            user.IsBand = 1;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new { response = user.UserName+" has been band"});
        }

    }
}