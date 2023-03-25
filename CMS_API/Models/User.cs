using System;
using System.Collections.Generic;

namespace CMS_API.Models
{
    public partial class User
    {
        public User()
        {
            Courses = new HashSet<Course>();
            EnrollCourses = new HashSet<EnrollCourse>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Phone { get; set; }
        public string? Major { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<EnrollCourse> EnrollCourses { get; set; }
    }
}
