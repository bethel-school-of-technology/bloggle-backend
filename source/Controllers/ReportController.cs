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
        public ActionResult<IEnumerable<Report>> Get()
        {
            List<Report> ReportList = _context.Reports.ToList();
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

            List<Report> ReportList = _context.Reports.ToList();
            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return "Report has been successfully updated";
        }
        [HttpDelete]
        public async Task<string> Delete(Report report){

            List<Report> PostList = _context.Reports.ToList();
            _context.Entry(report).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return "Your report has been deleted";
        }

    }
}
