namespace CMS_API.ControllerModels
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Phone { get; set; }
        public string? Major { get; set; }
    }
}
