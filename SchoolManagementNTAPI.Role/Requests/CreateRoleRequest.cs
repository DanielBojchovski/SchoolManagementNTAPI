namespace SchoolManagementNTAPI.Role.Requests
{
    public class CreateRoleRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Claims { get; set; } = new();
    }
}
