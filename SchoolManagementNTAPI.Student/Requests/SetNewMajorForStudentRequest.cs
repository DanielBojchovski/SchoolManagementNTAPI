namespace SchoolManagementNTAPI.Student.Requests
{
    public class SetNewMajorForStudentRequest
    {
        public int StudentId { get; set; }
        public int NewMajorId { get; set; }
    }
}
