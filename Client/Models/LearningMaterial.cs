using System;
using System.Collections.Generic;

namespace Client.Models
{
    public partial class LearningMaterial
    {
        public int LmId { get; set; }
        public string Title { get; set; }
        public string Information { get; set; }
        public string Url { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
