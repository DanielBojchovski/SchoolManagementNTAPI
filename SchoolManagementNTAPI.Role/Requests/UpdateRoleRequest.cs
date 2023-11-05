namespace SchoolManagementNTAPI.Role.Requests
{
    public class UpdateRoleRequest
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<string> Claims { get; set; } = new();
    }
}
