using SchoolManagementNTAPI.School.Models;

namespace SchoolManagementNTAPI.School.Responses
{
    public class GetAllSchoolsResponse
    {
        public List<SchoolModel> Lista { get; set; } = new();
    }
}
