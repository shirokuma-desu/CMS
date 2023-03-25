using System;
using System.Collections.Generic;

namespace CMS_API.Models
{
    public partial class UserDetail
    {
        public int UserDetailId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime? Dob { get; set; }
        public string? Phone { get; set; }
        public string Major { get; set; } = null!;
        public int? UserId { get; set; }

        public virtual User? User { get; set; }
    }
}
