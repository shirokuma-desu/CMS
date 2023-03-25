using CMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private CMSContext _context;

        public RoleController(CMSContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetRole()
        {
            var context = await _context.Users.Include(u=>u.Role).ToListAsync();
            return Ok(context);
        }
    }
}
