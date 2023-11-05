namespace SchoolManagementNTAPI.Authentication.Responses
{
    public class RefreshTokenResponse
    {
        public string Message { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
    }
}
