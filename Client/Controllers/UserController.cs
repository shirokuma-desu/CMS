using Client.Helper;
using Client.Models;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("Login")]
        public async Task<IActionResult> DoLogin(string username, string password)
        {
            // Call API
            var data = new LoginRequest { username = username, password = password };
            var response = await APIHelper.PostAsync<LoginRequest, LoginResponse>(_LOGIN_URL, data, null);

            // If request to server failed or response model equal null
            if(response == null)
            {
                ViewData["ErrorMessage"] = "Request to server failed";
                return View("Login");
            }

            // If request to server successfully
            // Append cookie to session
            HttpContext.Session.SetObjectAsJson("sessionAccount", new SessionAccount { Role = response.data.role, Token = response.data.token, Name = response.data.name, Email = response.data.email });

            return Redirect("/");
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

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
        public Data data { get; set; }
    }

    class Data
    {
        public string role { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string token { get; set; }
    }
}
