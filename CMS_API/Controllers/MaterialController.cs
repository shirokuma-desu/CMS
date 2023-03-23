using CMS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var context = await _context.LearningMaterials.Include(c=>c.Course).Where(c=> c.CourseId == course_id).ToListAsync();
            return Ok(context);
        }

        [HttpPost]
        public async Task<IActionResult> Post(LearningMaterial m)
        {
            _context.Add(m);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
