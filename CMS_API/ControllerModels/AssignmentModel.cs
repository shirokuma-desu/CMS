namespace CMS_API.ControllerModels
{
    public class AssignmentModel
    {
        public int AsignmentId { get; set; }
        public int? TeacherId { get; set; }
        public int? CourseId { get; set; }
        public string? Name { get; set; }
        public DateTime? Deadline { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        
    }
}
