namespace SchoolManagementNTAPI.AppUser.Responses
{
    public class GetUserByIdResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool LockoutEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string? Role { get; set; }
    }
}
