using Newtonsoft.Json;

namespace Client.Models.ViewModels
{
    public class CourseDetailsViewModel
    {
        [JsonProperty("course")]
        public Course Course { get; set; }
        [JsonProperty("teacher")]
        public Teacher Teacher { get; set; }
        [JsonProperty("isEnroll")]
        public bool IsEnrolled { get; set; }
    }
    public class Course
    {
        [JsonProperty("courseId")]
        public string CourseId { get; set; }
        [JsonProperty("courseName")]
        public string CourseName { get; set; }
        [JsonProperty("courseCode")]
        public string CourseCode { get; set; }
        [JsonProperty("courseDescription")]
        public string CourseDescription { get; set; }
        [JsonProperty("assignment")]
        public List<Assignment> Assignments { get; set; }
        [JsonProperty("material")]
        public List<Material> Materials { get; set; }
        [JsonProperty("numberOfClassmate")]
        public string NumberOfClassmate { get; set; }
    }

    public class Teacher
    {
        [JsonProperty("teacherId")]
        public string Id { get; set; }
        [JsonProperty("teacherName")]
        public string Name { get; set; }
    }

    public class Assignment
    {
        [JsonProperty("asignmentId")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("deadline")]
        public string Deadline { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public class Material
    {
        [JsonProperty("lmId")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        [JsonProperty("information")]
        public string Description { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
