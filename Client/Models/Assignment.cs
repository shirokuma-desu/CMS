using System;
using System.Collections.Generic;

namespace Client.Models
{
    public partial class Assignment
    {
        public Assignment()
        {
            Submissions = new HashSet<Submission>();
        }

        public int AsignmentId { get; set; }
        public int? TeacherId { get; set; }
        public string Name { get; set; }
        public DateTime? Deadline { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int? CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
