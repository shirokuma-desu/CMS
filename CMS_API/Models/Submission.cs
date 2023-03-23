﻿using System;
using System.Collections.Generic;

namespace CMS_API.Models
{
    public partial class Submission
    {
        public int Id { get; set; }
        public int? StudentJd { get; set; }
        public int? AssignmentId { get; set; }
        public DateTime? SubmissionTime { get; set; }
        public string? Url { get; set; }

        public virtual Assignment? Assignment { get; set; }
    }
}
