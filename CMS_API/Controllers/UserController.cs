using CMS_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CMS_API.JWTService;
using CMS_API.ControllerModels;
using Client.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var login = await _context.Users
                .Where(a => a.Email == loginModel.Username && a.Password == loginModel.Password).Include(r => r.Role)
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

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> getUserDetail()
        {
            var context = await _context.Users.ToListAsync();
            return Ok(context);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount([FromBody] UserProfileModel model)
        {
            User u = new User
            {
                Email = model.Email,
                Password = model.Password,
                RoleId = model.RoleId,
            };
            var context = await _context.Users.AddAsync(u);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "teacher,student")]
        public async Task<IActionResult> EditProfile([FromBody] UserProfileModel model, int id)
        {
            try
            {
                var tmp = await _context.Users.FindAsync(id);
                if (tmp == null)
                {
                    return NotFound();
                }

                tmp.Name = model.Name;
                tmp.Dob = model.Dob;
                tmp.Phone = model.Phone;
                tmp.Major = model.Major;

                _context.Entry(tmp).CurrentValues.SetValues(tmp);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var context = await _context.Users.FirstOrDefaultAsync(c => c.UserId == id);

                if (context != null)
                {
                    var c = _context.Users.Remove(context);
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

