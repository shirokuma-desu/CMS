namespace CMS_API.ControllerModels
{
    public class CourseModel
    {
        public int? CourseId { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; }
        public int TeacherId { get; set; }
    }
}
