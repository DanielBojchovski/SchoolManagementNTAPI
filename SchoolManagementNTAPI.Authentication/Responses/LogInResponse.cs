namespace SchoolManagementNTAPI.Authentication.Responses
{
    public class LogInResponse
    {
        public string Message { get; set; } = string.Empty;
        public string AuthToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
