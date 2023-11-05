namespace SchoolManagementNTAPI.Role.Responses
{
    public class GetRolesDropDownResponse
    {
        public List<NameOfRole> Lista { get; set; } = new();
    }
    public class NameOfRole
    {
        public string Name { get; set; } = string.Empty;
    }
}
