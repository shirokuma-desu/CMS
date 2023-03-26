using System;
using System.Collections.Generic;

namespace Client.Models
{
    public partial class Submission
    {
        public int IdSubmission { get; set; }
        public int? StudentJd { get; set; }
        public int? AssignmentId { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public string? Url { get; set; }

        public virtual Assignment? Assignment { get; set; }
    }
}
