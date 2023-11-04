using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Subject.Models;
using SchoolManagementNTAPI.Subject.Requests;
using SchoolManagementNTAPI.Subject.Responses;

namespace SchoolManagementNTAPI.Subject.Interfaces
{
    public interface ISubjectService
    {
        public Task<GetAllSubjectsResponse> GetAllSubjects();
        public Task<SubjectModel?> GetSubjectById(IdRequest request);
        public Task<OperationStatusResponse> CreateSubject(CreateSubjectRequest request);
        public Task<OperationStatusResponse> UpdateSubject(UpdateSubjectRequest request);
        public Task<OperationStatusResponse> DeleteSubject(IdRequest request);
    }
}
