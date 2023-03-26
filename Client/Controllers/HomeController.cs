using Client.Helper;
using Client.Models;
using Client.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _GET_STUDENT_ENROLL_COURSE_URL = "https://localhost:7087/api/Enroll/GetStudentEnrollCourse/";
        private readonly string _GET_TEACHER_COURSE_URL = "https://localhost:7087/api/Courses/GetCourseByTeacherId";
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(HomeViewModel viewModel)
        {
            var sessionAccount = HttpContext.Session.GetObjectFromJson<SessionAccount>("sessionAccount");
            // If user is authenticated
            if(sessionAccount != null)
            {
                // If logged user is student
                if(sessionAccount.Role == Commons.Constant.STUDENT_ROLE)
                {
                    // Find all student's courses
                    var response = await APIHelper.GetAsync<GetStudentEnrollCourseResponse>($"{_GET_STUDENT_ENROLL_COURSE_URL}", sessionAccount.Token);
                    if(response != null)
                        viewModel.EnrollCourses = new GetStudentEnrollCourseResponse { Data = response.Data };
                }
                // If logged user is teacher
                else if(sessionAccount.Role == Commons.Constant.TEACHER_ROLE)
                {
                    var response = await APIHelper.GetAsync<GetTeacherCourse>($"{_GET_TEACHER_COURSE_URL}?email={sessionAccount.Email}", sessionAccount.Token);
                    if(response != null)
                        viewModel.TeacherCourses = new GetTeacherCourse { Data = response.Data };
                }
            }

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}