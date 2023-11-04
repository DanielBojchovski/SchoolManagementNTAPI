using SchoolManagementNTAPI.Subject.Models;

namespace SchoolManagementNTAPI.Student.Requests
{
    public class CreateStudentRequest
    {
        public string Name { get; set; } = string.Empty;
        public List<SubjectInfo> Subjects { get; set; } = new();
    }
}
