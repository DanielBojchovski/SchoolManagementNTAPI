namespace SchoolManagementNTAPI.AppUser.Requests
{
    public class ChangePasswordRequest
    {
        public string Id { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
