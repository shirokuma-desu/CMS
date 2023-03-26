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
        [SwaggerOperation(Summary = "thua")]
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
        public async Task<IActionResult> Post(string name, string information, DateOnly date, string url, int course_id)
        {
            return Ok();
        }

    }
}
