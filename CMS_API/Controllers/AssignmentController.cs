using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Data;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private CMSContext _context;

        public AssignmentController(CMSContext context)
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

                if (context != null)
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
            var userId = int.Parse(User.Identity?.Name);
            Assignment asg = new Assignment
            {
                Name = model.Name,
                Url = model.Url,
                TeacherId = userId,
                Description = model.Description,
                Deadline = model.Deadline,
                CourseId = model.CourseId,
            };
            await _context.Assignments.AddAsync(asg);
            await _context.SaveChangesAsync();
            return Ok();
        }

        
        [HttpPatch("{id}")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Put(int id, [FromBody] AssignmentModel model)
        {
            try
            {
                var tmp = await _context.Assignments.FindAsync(id);
                if (tmp == null)
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
