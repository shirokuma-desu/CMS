using CMS_API.Models;
using Microsoft.AspNetCore.Mvc;
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
        
        // GET: api/<Course>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var context = await _context.Courses.Include(lm => lm.LearningMaterials).Include(a=>a.Assignments).ToListAsync();
            return Ok(context);
        }

        // GET api/<Course>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Course>
        [HttpPost]
        public async Task<IActionResult> Post(string coursename,string code,int teacher_id)
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

        // PUT api/<Course>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Course>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
