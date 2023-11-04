using SchoolManagementNTAPI.Professor.Models;

namespace SchoolManagementNTAPI.Professor.Responses
{
    public class GetAllProfessorsResponse
    {
        public List<ProfessorModel> Lista { get; set; } = new();
    }
}
