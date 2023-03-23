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


        [HttpPost]
        public async Task<IActionResult> PostMaterialbyTeacher(int id)
        {

        }
    }
}
