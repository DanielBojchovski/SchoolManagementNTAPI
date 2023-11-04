namespace SchoolManagementNTAPI.Subject.Requests
{
    public class UpdateSubjectRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
