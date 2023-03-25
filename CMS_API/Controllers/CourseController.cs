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
        public async Task<IActionResult> getAllCourse()
        {
            var context = await _context.Courses.ToListAsync();
            return Ok(context);
        }


        [HttpPost]
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> Put(int id, int teacher_id, string name, string code)
        {
            try
            {
                var tmp = await _context.Courses.FindAsync(id);
                if (tmp == null)
                {
                    return NotFound();
                }

                tmp.Code = code;
                tmp.Name = name;
                tmp.TeacherId = teacher_id;

                _context.Entry(tmp).CurrentValues.SetValues(tmp);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id);

                if (context != null)
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
