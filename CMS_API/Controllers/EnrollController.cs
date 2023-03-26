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

        [HttpGet("IsStudentEnrollCourse")]
        [Authorize(Roles = "student")]
<<<<<<< Updated upstream
        public async Task<IActionResult> enrollCourse(int courseId, [FromBody] Enroll_Course model )
=======
        public async Task<IActionResult> IsStudentEnrollCourse(string courseId)
>>>>>>> Stashed changes
        {
            var userId = int.Parse(User.Identity?.Name);

            EnrollCourse e = new EnrollCourse
            {
<<<<<<< Updated upstream
                StudentId = model.StudentId,
                CourseId = model.CourseId,
            };
            var courseIdexist = await _context.EnrollCourses.FirstOrDefaultAsync(c => c.CourseId == courseId);
            if (courseId == null)
            {
                await _context.EnrollCourses.AddAsync(e);
=======
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

                // Find enroll course with student id
                if(int.TryParse(courseId, out int convCourseId))
                {
                    var isStudentEnrolled = _context.EnrollCourses
                        .Where(x => x.StudentId == studentByEmail.UserId && x.CourseId == convCourseId)
                        .Count() > 0;

                    return Ok(new
                    {
                        IsEnroll = isStudentEnrolled
                    });
                }

                return Ok(new
                {
                    IsEnroll = false
                });
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetStudentEnrollCourse")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> GetStudentEnrollCourse()
        {
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

                // Find enroll course with student id
                var enrollCourseByStudent = await _context.EnrollCourses
                    .Where(x => x.StudentId == studentByEmail.UserId)
                    .Include(x => x.Course)
                    .Select(x => new
                    {
                        EnrollCourseId = x.IdEnrollCourse,
                        StudentId = x.StudentId,
                        CourseId = x.CourseId,
                        Course = new
                        {
                            Id = x.CourseId,
                            Name = x.Course.Name,
                            Code = x.Course.Code
                        }
                    })
                    .ToListAsync();

                if(enrollCourseByStudent.Count > 0)
                    return Ok(new { Data = enrollCourseByStudent });

                // Not found any student enroll course
                return Ok(new List<EnrollCourse>());
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("courseId")]
        [Authorize(Roles = "student")]
        public async Task<IActionResult> EnrollCourse([FromBody] EnrollCourseModel enrollCourse)
        {
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

                var courseId = int.Parse(enrollCourse.CourseId);
                var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                if(courseById == null)
                {
                    return BadRequest($"Course with id {courseId} not existed");
                }

                // Check if student already enrolled
                var alreadyEnrolled = await _context.EnrollCourses.FirstOrDefaultAsync(x => x.StudentId == studentByEmail.UserId && x.CourseId == courseId);
                if(alreadyEnrolled != null)
                {
                    // If student already enrolled then unenroll
                    _context.EnrollCourses.Remove(alreadyEnrolled);
                    await _context.SaveChangesAsync();

                    return Ok();
                }

                // Add enroll course
                await _context.EnrollCourses.AddAsync(new EnrollCourse
                {
                    StudentId = studentByEmail.UserId,
                    CourseId = courseId,
                });

>>>>>>> Stashed changes
                await _context.SaveChangesAsync();
                return Ok();
            }
<<<<<<< Updated upstream
            else
=======
            catch(Exception ex)
>>>>>>> Stashed changes
            {
                return BadRequest();
            }
        }

        [HttpDelete("courseId")]
        [Authorize(Roles = "student")]
<<<<<<< Updated upstream
        public async Task<IActionResult> unrollCourse(int courseId)
=======
        public async Task<IActionResult> UnrollCourse([FromBody] EnrollCourseModel enrollCourse)
>>>>>>> Stashed changes
        {
            var userId = int.Parse(User.Identity?.Name);
            try
            {
<<<<<<< Updated upstream

                var context = await _context.EnrollCourses.FirstOrDefaultAsync(c => c.CourseId == courseId && userId == c.StudentId);

                if (context != null)
=======
                var tokenEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                if(tokenEmail == null)
>>>>>>> Stashed changes
                {
                    var c = _context.EnrollCourses.Remove(context);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
<<<<<<< Updated upstream
                return NotFound();
            }
            catch
            (Exception ex)
=======

                var studentByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == tokenEmail);
                if(studentByEmail == null)
                {
                    return BadRequest($"Student with email {tokenEmail} not existed");
                }

                var courseId = int.Parse(enrollCourse.CourseId);
                var courseById = await _context.Courses.FirstOrDefaultAsync(x => x.CourseId == courseId);
                if(courseById == null)
                {
                    return BadRequest($"Course with id {courseId} not existed");
                }

                // Check if student already enrolled
                var alreadyEnrolled = await _context.EnrollCourses.FirstOrDefaultAsync(x => x.StudentId == studentByEmail.UserId && x.CourseId == courseId);
                if(alreadyEnrolled == null)
                {
                    return BadRequest($"Student with email {studentByEmail.Email} not enroll course with ID {courseById.CourseId}");
                }

                // Remove enroll course with student id and course id
                _context.EnrollCourses.Remove(alreadyEnrolled);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch(Exception ex)
>>>>>>> Stashed changes
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
