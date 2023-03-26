using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public async Task<IActionResult> enrollCourse([FromBody] EnrollCourseModel enrollCourse)
        {
            try
            {
                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (tokenEmail == null)
                {
                    return BadRequest($"Not found claim email from token");
                }

                var studentByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if (studentByEmail == null)
                {
                    return BadRequest($"Student with email {tokenEmail} not existed");
                }

                var courseId = int.Parse(enrollCourse.CourseId);
                var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                if (courseById == null)
                {
                    return BadRequest($"Course with id {courseId} not existed");
                }

                // Check if student already enrolled
                var alreadyEnrolled = await _context.EnrollCourses.FirstOrDefaultAsync(x => x.StudentId == studentByEmail.UserId && x.CourseId == courseId);
                if (alreadyEnrolled != null)
                {
                    return Ok();
                }

                // Add enroll course
                await _context.EnrollCourses.AddAsync(new EnrollCourse
                {
                    StudentId = studentByEmail.UserId,
                    CourseId = courseId,
                });

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("courseId")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> unrollCourse([FromBody] EnrollCourseModel enrollCourse)
        {
            try
            {
                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if (tokenEmail == null)
                {
                    return BadRequest($"Not found claim email from token");
                }

                var studentByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if (studentByEmail == null)
                {
                    return BadRequest($"Student with email {tokenEmail} not existed");
                }

                var courseId = int.Parse(enrollCourse.CourseId);
                var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                if (courseById == null)
                {
                    return BadRequest($"Course with id {courseId} not existed");
                }

                // Check if student already enrolled
                var alreadyEnrolled = await _context.EnrollCourses.FirstOrDefaultAsync(x => x.StudentId == studentByEmail.UserId && x.CourseId == courseId);
                if (alreadyEnrolled == null)
                {
                    return BadRequest($"Student with email {studentByEmail.Email} not enroll course with ID {courseById.CourseId}");
                }

                // Remove enroll course with student id and course id
                _context.EnrollCourses.Remove(alreadyEnrolled);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
