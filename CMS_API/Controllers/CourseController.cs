using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
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
                var context = await _context.Courses.Where(c=>c.Code.Contains(code.ToUpper())).ToListAsync();
                if (context == null)
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

        [HttpGet("getcoursebyid/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursebyId(int id)
        {
            try
            {
                var context = await _context.Courses.Include(u=>u.Teacher).FirstOrDefaultAsync(c => c.CourseId == id);
                if (context == null)
                {
                    return NoContent();
                }
                return Ok(context);
            }
            catch (Exception ex)
            {
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
                if (tmp == null)
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
            catch (Exception ex)
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
