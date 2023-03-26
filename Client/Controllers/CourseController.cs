using Client.Helper;
using Client.Models;
using Client.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Client.Controllers
{
    [Route("Courses")]
    public class CourseController : Controller
    {
        private readonly string _GET_COURSE_BY_ID_URL = "https://localhost:7087/api/Courses";
        private readonly string _IS_STUDENT_ENROLL_COURSE_URL = "https://localhost:7087/api/Enroll/IsStudentEnrollCourse";
        private readonly string _ENROLL_COURSE_URL = "https://localhost:7087/api/Enroll/courseId";

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(string id, CourseDetailsViewModel viewModel)
        {
            try
            {
                if(int.TryParse(id, out int convId))
                {
                    var getCourseDetailsResponse = await APIHelper.GetAsync<CourseDetailsViewModel>($"{_GET_COURSE_BY_ID_URL}/{convId}", null);

                    if(getCourseDetailsResponse != null)
                    {
                        viewModel.Course = getCourseDetailsResponse.Course;
                        viewModel.Teacher = getCourseDetailsResponse.Teacher;
                        viewModel.IsEnrolled = false;
                    }

                    var sessionAccount = HttpContext.Session.GetObjectFromJson<SessionAccount>("sessionAccount");
                    if(sessionAccount != null)
                    {
                        var isStudentEnrollResponse = await APIHelper.GetAsync<CourseDetailsViewModel>($"{_IS_STUDENT_ENROLL_COURSE_URL}?courseId={id}", sessionAccount.Token);
                        if(isStudentEnrollResponse != null)
                        {
                            viewModel.IsEnrolled = isStudentEnrollResponse.IsEnrolled;
                        }
                    }

                    return View(viewModel);
                }
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

            return RedirectPermanent("/");
        }

        [HttpGet("Enroll/{id}")]
        public async Task<IActionResult> Unenroll(string id)
        {
            try
            {
                if(int.TryParse(id, out int convId))
                {
                    var sessionAccount = HttpContext.Session.GetObjectFromJson<SessionAccount>("sessionAccount");
                    if(sessionAccount != null)
                    {
                        var unenrollResponse = await APIHelper.PostAsync<EnrollRequest, CourseDetailsViewModel>($"{_ENROLL_COURSE_URL}", new EnrollRequest { CourseId = id }, sessionAccount.Token);
                    }
                }
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
            }

            return RedirectToAction("Details", new { id = id });
        }

        class EnrollRequest
        {
            [JsonPropertyName("courseId")]
            public string CourseId { get; set; }
        }
    }
}
