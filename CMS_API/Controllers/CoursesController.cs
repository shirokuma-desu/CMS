using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private CMSContext _context;

        public CoursesController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourse()
        {
            var context = await _context.Courses.ToListAsync();
            return Ok(context);
        }
        [HttpGet("getcoursebycode/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursebyCode(string code)
        {
            try
            {
                var context = await _context.Courses.Where(c => c.Code.Contains(code.ToUpper())).ToListAsync();
                if(context == null)
                {
                    return NoContent();
                }
                return Ok(context);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetCourseByTeacherId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseByTeacherId(string? email)
        {
            try
            {
                var teacherByEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if(teacherByEmail == null)
                {
                    return BadRequest($"Teacher with email {email} not existed");
                }

                // Find course by teacher id
                var courseByTeacherId = await _context.Courses
                    .Where(x => x.TeacherId == teacherByEmail.UserId)
                    .Select(x => new
                    {
                        Id = x.CourseId,
                        Name = x.Name,
                        Code = x.Code
                    })
                    .ToListAsync();

                if(courseByTeacherId.Count() > 0)
                    return Ok(new
                    {
                        Data = courseByTeacherId
                    });

                // Not found any course with teacher id
                return Ok(new List<Course> { });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCourseById(int id)
        {
            try
            {
                // Find course by id
                var courseById = await _context.Courses
                    .Include(x => x.Teacher)
                    .Include(x => x.Assignments)
                    .Include(x => x.LearningMaterials)
                    .Include(x => x.EnrollCourses)
                    .FirstOrDefaultAsync(x => x.CourseId == id);
                if(courseById == null)
                    return NoContent();

                // Get list of assignments of from courseById
                var assignmentByCourse = courseById.Assignments
                    .Select(x => new
                    {
                        x.AsignmentId,
                        x.Name,
                        x.Deadline,
                        x.Description,
                        x.Url
                    })
                    .ToList();

                // Get list of materials of from courseById
                var materialByCourse = courseById.LearningMaterials
                    .Select(x => new
                    {
                        x.LmId,
                        x.Title,
                        x.Information,
                        x.Url
                    })
                    .ToList();

                return Ok(new
                {
                    Course = new
                    {
                        CourseId = courseById.CourseId,
                        CourseName = courseById.Name,
                        CourseCode = courseById.Code,
                        CourseDescription = "Some quick example text to build on the card title and make up the bulk the card's content.",
                        Assignment = assignmentByCourse,
                        Material = materialByCourse,
                        NumberOfClassmate = courseById.EnrollCourses.Count()
                    },
                    Teacher = new
                    {
                        TeacherId = courseById.TeacherId,
                        TeacherName = courseById.Teacher.Name
                    }
                });
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Post([FromBody] CourseModel model)
        {
            Course c = new Course
            {
                Code = model.Code,
                Name = model.Name,
                TeacherId = model.TeacherId,
            };
            var context = await _context.Courses.AddAsync(c);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Put(int id, [FromBody] CourseModel model)
        {
            try
            {
                var tmp = await _context.Courses.FindAsync(id);
                if(tmp == null)
                {
                    return NotFound();
                }

                tmp.Code = model.Code;
                tmp.Name = model.Name;
                tmp.TeacherId = model.TeacherId;

                _context.Entry(tmp).CurrentValues.SetValues(tmp);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);

                if(context != null)
                {
                    var c = _context.Courses.Remove(context);
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
