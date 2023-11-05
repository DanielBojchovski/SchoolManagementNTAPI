using SchoolManagementNTAPI.Common.Responses;
using SchoolManagementNTAPI.Role.Requests;
using SchoolManagementNTAPI.Role.Responses;

namespace SchoolManagementNTAPI.Role.Interfaces
{
    public interface IRolesService
    {
        public Task<OperationStatusResponse> CreateRole(CreateRoleRequest request);
        public Task<OperationStatusResponse> UpdateRole(UpdateRoleRequest request);
        public Task<GetRolesResponse> GetRoles();
        public Task<GetRoleByIdResponse> GetRoleById(IdRoleRequest request);
        public Task<OperationStatusResponse> DeleteRole(IdRoleRequest request);
        public GetAllClaimsResponse GetAllClaims();
        public Task<GetRolesDropDownResponse> GetRolesDropDown();
    }
}
