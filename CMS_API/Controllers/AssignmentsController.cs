using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;
using System.Security.Claims;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private CMSContext _context;

        public AssignmentsController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var context = await _context.Assignments.Include(s => s.Submissions).ToListAsync();
            return Ok(context);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "teacher")]
        [SwaggerOperation(Summary = "xoa bai tap")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Assignments.FirstOrDefaultAsync(c => c.AsignmentId == id);

                if(context != null)
                {
                    var c = _context.Assignments.Remove(context);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest("Null");
            }
            catch
            (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Post([FromBody] AssignmentModel model)
        {
            try
            {
                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if(tokenEmail == null)
                {
                    return BadRequest($"Not found claim email from token");
                }

                var teacherByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if(teacherByEmail == null)
                {
                    return BadRequest($"Teacher with email {tokenEmail} not existed");
                }

                if(int.TryParse(model.CourseId, out var courseId))
                {
                    var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                    if(courseById == null)
                    {
                        return BadRequest($"Course with ID {courseById} not existed");
                    }

                    // Create assignment
                    await _context.Assignments.AddAsync(new Assignment
                    {
                        TeacherId = teacherByEmail.UserId,
                        CourseId = courseById.CourseId,
                        Name = model.Name,
                        Deadline = model.Deadline,
                        Description = model.Description,
                        Url = model.Url
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


        [HttpPatch("{id}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Put(int id, [FromBody] AssignmentModel model)
        {
            try
            {
                var tmp = await _context.Assignments.FindAsync(id);
                if(tmp == null)
                {
                    return NotFound();
                }

                tmp.Description = model.Description;
                tmp.Name = model.Name;
                tmp.Deadline = model.Deadline;
                tmp.Url = model.Url;
                _context.Entry(tmp).CurrentValues.SetValues(tmp);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
