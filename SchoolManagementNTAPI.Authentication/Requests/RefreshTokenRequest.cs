namespace SchoolManagementNTAPI.Authentication.Requests
{
    public class RefreshTokenRequest
    {
        public string AuthToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
    }
}
