using System;
using System.Collections.Generic;

namespace CMS_API.Models
{
    public partial class EnrollCourse
    {
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public int Id { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? Student { get; set; }
    }
}
