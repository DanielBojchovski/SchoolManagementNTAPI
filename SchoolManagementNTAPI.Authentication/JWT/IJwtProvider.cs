using System.Security.Claims;

namespace SchoolManagementNTAPI.Authentication.JWT
{
    public interface IJwtProvider
    {
        RefreshTokenDTO GenerateRefreshToken(string userId);
        string GenerateToken(JWTModel jwtModel);
        string RefreshToken(List<Claim> tokenClaims);
    }
}
