namespace SchoolManagementNTAPI.Role.Responses
{
    public class GetRoleByIdResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> ClaimsForRole { get; set; } = new();
    }
}
