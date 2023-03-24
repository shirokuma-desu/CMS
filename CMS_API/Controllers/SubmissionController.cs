using CMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private CMSContext _context;

        public SubmissionController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubByStudent(int student_id, int assignment_id)
        {
            
            var subs = await _context.Submissions.FirstOrDefaultAsync(s => s.StudentJd == student_id && s.AssignmentId == assignment_id);
            if (subs == null)
            {
                return NotFound();
            }
            return Ok(subs);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Submission s)
        {
            var sub = await _context.Submissions.AddAsync(s);
            return Ok(sub);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Submissions.FirstOrDefaultAsync(c => c.Id == id);

                if (context != null)
                {
                    var c = _context.Submissions.Remove(context);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch
            (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
    
}
