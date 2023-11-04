namespace SchoolManagementNTAPI.Principal.Requests
{
    public class CreatePrincipalRequest
    {
        public string Name { get; set; } = string.Empty;
        public int SchoolId { get; set; }
    }
}
