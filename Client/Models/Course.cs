using System;
using System.Collections.Generic;

namespace Client.Models
{
    public partial class Course
    {
        public Course()
        {
            Assignments = new HashSet<Assignment>();
            EnrollCourses = new HashSet<EnrollCourse>();
            LearningMaterials = new HashSet<LearningMaterial>();
        }

        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int? TeacherId { get; set; }

        public virtual User Teacher { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<EnrollCourse> EnrollCourses { get; set; }
        public virtual ICollection<LearningMaterial> LearningMaterials { get; set; }
    }
}
