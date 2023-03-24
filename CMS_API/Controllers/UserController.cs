/*using CMS_API.Models;
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

        public UserController(CMSContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> LoginTeacher([FromBody] LoginModel loginModel)
        {
            var login = await _context.Users
                .Where(a => a.Email == loginModel.Email && a.Password == loginModel.Password)
                .FirstOrDefaultAsync();

            if (login == null)
                return Unauthorized(new
                {
                    status = "Unauthorized",
                    message = "Username or password is wrong",
                    data = ""
                });

            var jwtToken = CreateAccount.CreateToken(login);
            // chỗ này m gọi class tạo token
            return Ok(new
            {
                status = "Success",
                message = "User logged in successfully",
                data = new
                {
                    id = User,
                    token = jwtToken
                }
            });
        }
    }
}
*/