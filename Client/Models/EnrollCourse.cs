using System;
using System.Collections.Generic;

namespace Client.Models
{
    public partial class EnrollCourse
    {
        public int? StudentId { get; set; }
        public int? CourseId { get; set; }
        public int IdEnrollCourse { get; set; }

        public virtual Course? Course { get; set; }
        public virtual User? Student { get; set; }
    }
}
