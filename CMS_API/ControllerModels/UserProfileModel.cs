namespace Client.Models
{
    public class UserProfileModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public DateTime? Dob { get; set; }
        public string Phone { get; set; }
        public string Major { get; set; }
    }
}
