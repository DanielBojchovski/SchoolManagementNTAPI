namespace SchoolManagementNTAPI.AppUser.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string? Role { get; set; }
    }
}
