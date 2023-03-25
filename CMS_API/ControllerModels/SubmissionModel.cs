namespace CMS_API.ControllerModels
{
    public class SubmissionModel
    {
        public int IdSubmission { get; set; }
        public int? StudentJd { get; set; }
        public int? AssignmentId { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public string? Url { get; set; }
    }
}
