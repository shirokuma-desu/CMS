using Client.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Controllers
{
    public class CourseController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly HttpClient client = null;
        private string CourseApiUrl = "";

        public CourseController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
        }

        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response1 = await client.GetAsync("https://localhost:7087/api/Course");
            string strData1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Course> listCourse = JsonSerializer.Deserialize<List<Course>>(strData1, options1);
            return View(listCourse); 
        }

        [HttpPost]
        public async Task<IActionResult> Index(string search)
        {
            HttpResponseMessage response1 = await client.GetAsync("https://localhost:7087/api/Course/getcoursebycode/" + search);
            string strData1 = await response1.Content.ReadAsStringAsync();
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<Course> listCourse = JsonSerializer.Deserialize<List<Course>>(strData1, options1);
            List<Course> searchListCourse = listCourse.FindAll(c => c.Code.Equals(search)).ToList();
            return View(searchListCourse);
        }

        public async Task<IActionResult> Details(int id) 
        {
            HttpResponseMessage response = await client.GetAsync("https://localhost:7087/api/Course/getcoursebyid/" + id);
            string strData = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Course? course = JsonSerializer.Deserialize<Course>(strData, options);
            return View(course);
        }
    }
}
