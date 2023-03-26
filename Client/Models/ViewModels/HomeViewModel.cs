using System.Text.Json.Serialization;

namespace Client.Models.ViewModels
{
    public class HomeViewModel
    {
        public GetStudentEnrollCourseResponse? EnrollCourses { get; set; }
        public GetTeacherCourse? TeacherCourses { get; set; }
    }

    // STUDENT VIEW MODEL
    public class GetStudentEnrollCourseResponse
    {
        [JsonPropertyName("data")]
        public List<EnrollCourseResponse>? Data { get; set; }
    }

    public class EnrollCourseResponse
    {
        [JsonPropertyName("enrollCourseId")]
        public int EnrollCourseId { get; set; }
        [JsonPropertyName("studentId")]
        public int StudentId { get; set; }
        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }
        [JsonPropertyName("course")]
        public CourseResponse? Course { get; set; }
    }

    public class CourseResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }

    // TEACHER VIEW MODEL

    public class GetTeacherCourse
    {
        [JsonPropertyName("data")]
        public List<CourseResponse>? Data { get; set; }
    }
}
