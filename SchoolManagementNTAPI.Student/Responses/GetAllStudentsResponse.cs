using SchoolManagementNTAPI.Student.Models;

namespace SchoolManagementNTAPI.Student.Responses
{
    public class GetAllStudentsResponse
    {
        public List<StudentModel> Lista { get; set; } = new();
    }
}
