using CMS_API.ControllerModels;
using CMS_API.Helper;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        [HttpPost("{assignmentId}")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> AssignmentSubmission(string assignmentId,[FromForm] IFormFile postedFile)
        {
            await Console.Out.WriteLineAsync("HIT BE");
            try
            {
                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if(tokenEmail == null)
                {
                    return BadRequest($"Not found claim email from token");
                }

                var studentByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if(studentByEmail == null)
                {
                    return BadRequest($"Student with email {tokenEmail} not existed");
                }

                if(int.TryParse(assignmentId, out var convAssignmentId))
                {
                    // Find assignment by id
                    var assignmentById = await _context.Assignments.FirstOrDefaultAsync(x => x.AsignmentId == convAssignmentId);

                    if(assignmentById == null)
                        return BadRequest($"Assignment with {convAssignmentId} is not exist");

                    // Check if submission time is before deadline
                    var submissionTime = DateTime.Now;
                    if(submissionTime > assignmentById.Deadline)
                        return BadRequest($"Submission deadline has expired");

                    // Save file to server
                    var uploadPathRs = await DirectoryHelper.ProcessUploadFile(postedFile, "Assignment", "AssignmentSubmission_");
                    if(uploadPathRs.StartsWith("Error: "))
                    {
                        return BadRequest(uploadPathRs);
                    }

                    // Write submission path to database
                    await _context.Submissions.AddAsync(new Submission
                    {
                        AssignmentId = convAssignmentId,
                        StudentJd = studentByEmail.UserId,
                        SubmissionTime = DateTime.Now,
                        Url = uploadPathRs
                    });

                    await _context.SaveChangesAsync();

                    return Ok();
                }

                return BadRequest("Something bad happened");
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("course/{id}/teacher")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> getListSubByStudent(int assignment_id)
        {

            var subs = await _context.Submissions.Where(s => s.AssignmentId == assignment_id).ToListAsync();
            if(subs == null)
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
            if(subs == null)
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
                if(tmp == null)
                {
                    return NotFound();
                }
                var time = await _context.Assignments.FindAsync(tmp.AssignmentId);
                if(time == null)
                {
                    return NotFound();
                }
                tmp.SubmissionTime = model.SubmissionTime;
                tmp.Url = model.Url;
                if(model.SubmissionTime > time.Deadline)
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
            catch(Exception ex)
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

                if(context != null)
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
                if(tmp == null)
                {
                    return NotFound();
                }
                var time = await _context.Assignments.FindAsync(tmp.AssignmentId);
                if(time == null)
                {
                    return NotFound();
                }
                tmp.SubmissionTime = model.SubmissionTime;
                tmp.Url = model.Url;
                if(model.SubmissionTime > time.Deadline)
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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }

}
