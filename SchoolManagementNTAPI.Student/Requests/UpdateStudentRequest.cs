using SchoolManagementNTAPI.Subject.Models;

namespace SchoolManagementNTAPI.Student.Requests
{
    public class UpdateStudentRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubjectInfo> Subjects { get; set; } = new();
    }
}
