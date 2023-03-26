using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> getListSubByStudent(int assignment_id)
        {

            var subs = await _context.Submissions.Where(s=>s.AssignmentId == assignment_id).ToListAsync();
            if (subs == null)
            {
                return NotFound();
            }
            return Ok(subs);
        }

        [HttpGet("student/{assignment_id}")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetSubByStudent(int assignment_id)
        {
            var student_id = int.Parse(User.Identity?.Name);
            var subs = await _context.Submissions.FirstOrDefaultAsync(s => s.StudentJd == student_id && s.AssignmentId == assignment_id);
            if (subs == null)
            {
                return NotFound();
            }
            return Ok(subs);
        }



        [HttpPost]
        [Authorize(Roles = "student")]
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
            try
            {
                var tmp = await _context.Submissions.FindAsync(userId);
                if (tmp == null)
                {
                    return NotFound();
                }
                var time = await _context.Assignments.FindAsync(tmp.AssignmentId);
                if (time == null)
                {
                    return NotFound();
                }
                tmp.SubmissionTime = model.SubmissionTime;
                tmp.Url = model.Url;
                if (model.SubmissionTime > time.Deadline)
                {
                    await _context.Submissions.AddAsync(s);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("not allow ");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "student")]
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

        [HttpPatch]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> Edit([FromBody] SubmissionModel model)
        {
            var userId = int.Parse(User.Identity?.Name);
            try
            {
                var tmp = await _context.Submissions.FindAsync(userId); 
                if (tmp == null)
                {
                    return NotFound();
                }
                var time =await _context.Assignments.FindAsync(tmp.AssignmentId);    
                if (time == null)
                {
                    return NotFound();
                }
                tmp.SubmissionTime = model.SubmissionTime;
                tmp.Url = model.Url;
                if (model.SubmissionTime > time.Deadline)
                {
                    _context.Entry(tmp).CurrentValues.SetValues(tmp);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest("not allow ");
                }
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
