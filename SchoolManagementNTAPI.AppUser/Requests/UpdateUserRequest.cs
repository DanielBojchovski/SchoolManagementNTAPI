namespace SchoolManagementNTAPI.AppUser.Requests
{
    public class UpdateUserRequest
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool LockoutEnabled { get; set; }
        public string? Role { get; set; }
    }
}
