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
    public class EnrollController : ControllerBase
    {
        private CMSContext _context;

        public EnrollController(CMSContext context)
        {
            _context = context;
        }

        [HttpPost("courseId")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> enrollCourse(int courseId, [FromBody] Enroll_Course model )
        {
            var userId = int.Parse(User.Identity?.Name);

            EnrollCourse e = new EnrollCourse
            {
                StudentId = model.StudentId,
                CourseId = model.CourseId,
            };
            var courseIdexist = await _context.EnrollCourses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (courseId == null)
            {
                await _context.EnrollCourses.AddAsync(e);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("courseId")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> unrollCourse(int courseId)
        {
            var userId = int.Parse(User.Identity?.Name);
            try
            {

                var context = await _context.EnrollCourses.FirstOrDefaultAsync(c => c.CourseId == courseId && userId == c.StudentId);

                if (context != null)
                {
                    var c = _context.EnrollCourses.Remove(context);
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
