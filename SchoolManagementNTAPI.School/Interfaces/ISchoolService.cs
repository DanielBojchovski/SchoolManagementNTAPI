using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.School.Models;
using SchoolManagementNTAPI.School.Requests;
using SchoolManagementNTAPI.School.Responses;

namespace SchoolManagementNTAPI.School.Interfaces
{
    public interface ISchoolService
    {
        public Task<GetAllSchoolsResponse> GetAllSchools();
        public Task<SchoolModel?> GetSchoolById(IdRequest request);
        public Task<SchoolModel?> GetSchoolByPrincipalId(IdRequest request);
        public Task<SchoolModel?> GetSchoolByProfessorId(IdRequest request);
        public Task<OperationStatusResponse> CreateSchool(CreateSchoolRequest request);
        public Task<OperationStatusResponse> UpdateSchool(UpdateSchoolRequest request);
        public Task<OperationStatusResponse> DeleteSchool(IdRequest request);
    }
}
