using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CMS_API.ControllerModels
{
    public class AssignmentModel
    {
        [JsonPropertyName("courseId")]
        public string? CourseId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("deadline")]
        public DateTime? Deadline { get; set; }
        [JsonPropertyName("description")]
        public string? Description { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
