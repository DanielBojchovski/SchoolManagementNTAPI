using SchoolManagementNTAPI.Common.Requests;
using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Principal.Models;
using SchoolManagementNTAPI.Principal.Requests;
using SchoolManagementNTAPI.Principal.Responses;

namespace SchoolManagementNTAPI.Principal.Interfaces
{
    public interface IPrincipalService
    {
        public Task<GetAllPrincipalsResponse> GetAllPrincipals();
        public Task<PrincipalModel?> GetPrincipalById(IdRequest request);
        public Task<PrincipalModel?> GetPrincipalBySchoolId(IdRequest request);
        public Task<OperationStatusResponse> CreatePrincipal(CreatePrincipalRequest request);
        public Task<OperationStatusResponse> UpdatePrincipal(UpdatePrincipalRequest request);
        public Task<OperationStatusResponse> DeletePrincipal(IdRequest request);
    }
}
