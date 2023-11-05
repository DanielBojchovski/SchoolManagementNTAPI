using Microsoft.AspNetCore.Http;
using SchoolManagementNTAPI.Authentication.Requests;
using SchoolManagementNTAPI.Authentication.Responses;
using SchoolManagementNTAPI.Common.Responses;

namespace SchoolManagementNTAPI.Authentication.Interfaces
{
    public interface IAuthService
    {
        Task<LogInResponse> Login(LoginRequest request);
        Task<OperationStatusResponse> LogOut(LogOutRequest request);
        Task<RefreshTokenResponse> RefreshAuthToken(RefreshTokenRequest request);
        Task<OperationStatusResponse> ChangePassword(ChangePasswordUserRequest request);
        RefreshTokenRequest GetTokensFromRequest(HttpRequest request);
    }
}
