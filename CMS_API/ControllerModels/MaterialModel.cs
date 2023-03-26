using CMS_API.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CMS_API.ControllerModels
{
    public class MaterialModel
    {
        [JsonPropertyName("courseId")]
        public string? CourseId { get; set; }
        [JsonPropertyName("materialName")]
        public string? Title { get; set; }
        [JsonPropertyName("materialDesc")]
        public string? Description { get; set; }
        [JsonPropertyName("materialUpload")]
        public IFormFile? PostedFile { get; set; }
    }
}
