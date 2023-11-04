using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Professor.Models;
using SchoolManagementNTAPI.Professor.Requests;
using SchoolManagementNTAPI.Professor.Responses;

namespace SchoolManagementNTAPI.Professor.Interfaces
{
    public interface IProfessorService
    {
        public Task<GetAllProfessorsResponse> GetAllProfessors();
        public Task<ProfessorModel?> GetProfessorById(IdRequest request);
        public Task<GetAllProfessorsResponse> GetProfessorsBySchoolId(IdRequest request);
        public Task<OperationStatusResponse> CreateProfessor(CreateProfessorRequest request);
        public Task<OperationStatusResponse> UpdateProfessor(UpdateProfessorRequest request);
        public Task<OperationStatusResponse> DeleteProfessor(IdRequest request);
    }
}
