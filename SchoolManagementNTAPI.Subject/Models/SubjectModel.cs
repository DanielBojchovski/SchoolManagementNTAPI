using SchoolManagementNTAPI.Common.Models;

namespace SchoolManagementNTAPI.Subject.Models
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<StudentDto> Students { get; set; } = new();
    }
}
