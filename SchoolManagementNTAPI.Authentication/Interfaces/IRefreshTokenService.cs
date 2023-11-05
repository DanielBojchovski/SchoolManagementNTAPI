using SchoolManagementNTAPI.Authentication.JWT;

namespace SchoolManagementNTAPI.Authentication.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<int> DeleteRefreshTokensForUser(string userId);
        Task<RefreshTokenDTO> SaveRefreshTokensForUser(RefreshTokenDTO refreshToken);
    }
}
