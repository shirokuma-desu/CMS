using CMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private CMSContext _context;

        public UserDetailsController(CMSContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> getAllUser()
        {
            var context = await _context.UserDetails.ToListAsync();
            return Ok(context);
        }
        
    }
}
