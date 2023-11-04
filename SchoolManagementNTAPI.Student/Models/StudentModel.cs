using SchoolManagementNTAPI.Common.Models;

namespace SchoolManagementNTAPI.Student.Models
{
    public class StudentModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<SubjectDto> Subjects { get; set; } = new();
    }
}
