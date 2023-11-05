using SchoolManagementNTAPI.Role.Models;

namespace SchoolManagementNTAPI.Role.Responses
{
    public class GetRolesResponse
    {
        public List<RoleModel> Lista { get; set; } = new();
    }
}
