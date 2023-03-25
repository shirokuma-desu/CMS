using CMS_API.ControllerModels;
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

        [HttpGet("course/{id}/teacher")]
        public async Task<IActionResult> getListSubByStudent(int assignment_id)
        {

            var subs = await _context.Submissions.Where(s=>s.AssignmentId == assignment_id).ToListAsync();
            if (subs == null)
            {
                return NotFound();
            }
            return Ok(subs);
        }

        [HttpGet("student")]
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
        public async Task<IActionResult> Submit([FromBody] SubmissionModel model)
        {
            var userId = int.Parse(User.Identity?.Name);
            Submission s = new Submission
            {
                Url = model.Url,
                SubmissionTime = model.SubmissionTime,
                AssignmentId = model.AssignmentId,
                StudentJd = userId,
            };
            await _context.Submissions.AddAsync(s);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Submissions.FirstOrDefaultAsync(c => c.IdSubmission == id);

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
