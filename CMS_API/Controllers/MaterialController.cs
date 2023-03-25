using CMS_API.ControllerModels;
using CMS_API.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Post([FromBody] MaterialModel model)
        {
            LearningMaterial lm = new LearningMaterial
            {
                CourseId = model.CourseId,
                Title = model.Title,
                Url = model.Url,
                Information = model.Information,
            };
            if (lm == null)
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

        [HttpPatch("id")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Put(int id, [FromBody] MaterialModel model)
        {
            try
            {
                var tmp = await _context.LearningMaterials.FindAsync(id);
                if (tmp == null)
                {
                    return NotFound();
                }

                tmp.Title = model.Title;
                tmp.Information = model.Information;
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

        [HttpDelete("id")]
        [Authorize(Roles = "teacher")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.LearningMaterials.SingleOrDefaultAsync(c => c.LmId == id);

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
