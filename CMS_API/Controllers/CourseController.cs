using CMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private CMSContext _context;

        public CourseController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var context = await _context.Courses.Include(lm => lm.LearningMaterials).Include(a => a.Assignments).Include(u => u.Teacher).ToListAsync();
            return Ok(context);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseId(int id)
        {
            try
            {
                var context = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
                return Ok(context);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateCourse/{teacher_id}")]
        public async Task<IActionResult> Post(string coursename, string code, int teacher_id)
        {
            Course c = new Course
            {
                Code = code,
                Name = coursename,
                TeacherId = teacher_id,
            };
            var context = await _context.Courses.AddAsync(c);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var context = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);

                var c = _context.Courses.Remove(context);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch
            (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
