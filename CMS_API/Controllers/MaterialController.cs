using CMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private CMSContext _context;

        public MaterialController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var context = await _context.LearningMaterials.ToListAsync();
            return Ok(context);
        }
        [HttpGet("{course_id}")]
        public async Task<IActionResult> getListMaterialByCourse(int course_id)
        {
            var context = await _context.LearningMaterials.Include(c => c.Course).Where(c => c.CourseId == course_id).ToListAsync();
            return Ok(context);
        }

        [HttpPost]
        public async Task<IActionResult> Post(string title,string information,string url,int course_id)
        {
            LearningMaterial lm = new LearningMaterial
            {
                CourseId = course_id,
                Title = title,
                Url = url,
                Information = information,
            };
            if(lm == null)
            {
                return NoContent();
            }
            try
            {
                await _context.LearningMaterials.AddAsync(lm);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (SqlException ex) 
            {
                return BadRequest(ex);
            }
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.LearningMaterials.SingleOrDefaultAsync(c => c.Id == id);

                if (context != null)
                {
                    var c = _context.LearningMaterials.Remove(context);
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
