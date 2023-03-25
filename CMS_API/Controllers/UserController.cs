using CMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS_API.JWTService;

namespace CMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private CMSContext _context;
        private ITokenService _tokenService;

        public UserController(CMSContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var login = await _context.Users
                .Where(a => a.Email == loginModel.Username && a.Password == loginModel.Password).Include(r=>r.Role)
                .FirstOrDefaultAsync();

            if (login == null)
                return Unauthorized(new
                {
                    status = "Unauthorized",
                    message = "Username or password is wrong",
                    data = ""
                });

            var jwtToken = _tokenService.CreateToken(login);
            // chỗ này m gọi class tạo token
            return Ok(new
            {
                status = "Success",
                message = "User logged in successfully",
                data = new
                {
                    token = jwtToken
                }
            });
        }
    }
}
