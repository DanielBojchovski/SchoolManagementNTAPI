namespace SchoolManagementNTAPI.Student.Responses
{
    public class GetStudentWithHisMajorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Major { get; set; } = string.Empty;
    }
}
