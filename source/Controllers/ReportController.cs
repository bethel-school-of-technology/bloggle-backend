using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using collaby_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace collaby_backend.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/reports
        [HttpGet]
        public ActionResult<IEnumerable<Report>> GetAll()
        {
            List<Report> ReportList = _context.Reports.OrderByDescending(o=>o.DateCreated).ToList();
            return ReportList;
        }

        //GET api/reports/id
        [HttpGet("{reportId}")]
        public ActionResult<Report> Get(long reportId)
        {
            Report report = _context.Reports.First(o=>o.Id == reportId);
            return report;
        }

        //GET api/reports/postId
        [HttpGet("post/{postId}")]
        public ActionResult<IEnumerable<Report>> GetPostReports(long postId)
        {
            List<Report> ReportList = _context.Reports.Where(o=>o.PostId == postId).OrderByDescending(o=>o.DateCreated).ToList();
            return ReportList;
        }

        [HttpPost]
        public async Task<string> POST(Report post){

            _context.Reports.Add(post);
            await _context.SaveChangesAsync();
            return "Report has been successfully added";
        }

        [HttpPut]
        public async Task<string> Edit(Report report){

            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Report has been successfully updated";
        }

        [HttpDelete]
        public async Task<string> Delete(Report report){

            _context.Entry(report).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return "Your report has been deleted";
        }

    }
}
