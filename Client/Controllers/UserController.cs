using Client.Helper;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

namespace Client.Controllers
{
    [Route("Users")]
    public class UserController : Controller
    {
        private readonly string _LOGIN_URL = "https://localhost:7087/api/User/login";
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("DoLogin")]
        public async Task<IActionResult> DoLogin(string username, string password)
        {
            // Call API
            var data = new LoginRequest { username =  username, password = password };
            var response = await APIHelper.PostAsync<LoginRequest, LoginResponse>(_LOGIN_URL, data);
            
            // If request to server failed or response model equal null
            if(response == null)
            {
                ViewData["ErrorMessage"] = "Request to server failed";
                return View("Login");
            }

            await Console.Out.WriteLineAsync(response.token);

            // If request to server successfully
            // Append cookie to session
            HttpContext.Session.SetString("sessionAccount", JsonConvert.SerializeObject(new SessionAccount { Role = "User", Token = response.token} ));

            return Redirect("/");
        }
    }

    class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    class LoginResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string token { get; set; }   
    }
}
