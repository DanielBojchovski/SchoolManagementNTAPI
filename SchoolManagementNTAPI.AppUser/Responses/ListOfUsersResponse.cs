using SchoolManagementNTAPI.AppUser.Models;

namespace SchoolManagementNTAPI.AppUser.Responses
{
    public class ListOfUsersResponse
    {
        public List<UserModel> Lista { get; set; } = new();
    }
}
