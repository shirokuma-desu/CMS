using CMS_API.Models;

namespace CMS_API.ControllerModels
{
    public class MaterialModel
    {
        public string? Title { get; set; }
        public string? Information { get; set; }
        public string? Url { get; set; }
        public int? CourseId { get; set; }
    }
}
