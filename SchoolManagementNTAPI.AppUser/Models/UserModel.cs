namespace SchoolManagementNTAPI.AppUser.Models
{
    public class UserModel
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? Role { get; set; }
    }
}
