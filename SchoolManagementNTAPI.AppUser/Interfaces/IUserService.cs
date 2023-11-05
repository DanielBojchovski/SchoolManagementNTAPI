using SchoolManagementNTAPI.AppUser.Requests;
using SchoolManagementNTAPI.AppUser.Responses;
using SchoolManagementNTAPI.Common.Responses;

namespace SchoolManagementNTAPI.AppUser.Interfaces
{
    public interface IUserService
    {
        public Task<OperationStatusResponse> CreateUser(CreateUserRequest request);
        public Task<OperationStatusResponse> Update(UpdateUserRequest request);
        public Task<ListOfUsersResponse> ListOfUsers();
        public Task<GetUserByIdResponse> GetUserById(IdUserRequest request);
        public Task<OperationStatusResponse> ResetLockout(IdUserRequest request);
        public Task<OperationStatusResponse> ToogleUser(IdUserRequest request);
        public Task<OperationStatusResponse> ChangePassword(ChangePasswordRequest request);
        public Task<UserDropDownResponse> UsersDropDown();
    }
}
