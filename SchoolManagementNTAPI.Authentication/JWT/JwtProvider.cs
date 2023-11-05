using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementNTAPI.Authentication.JWT.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolManagementNTAPI.Authentication.JWT
{
    public class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions _jwtOptions;

        public JwtProvider(IOptions<JwtOptions> options)
        {
            _jwtOptions = options.Value;
        }

        public string GenerateToken(JWTModel jWTGenerateTokenModel)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, jWTGenerateTokenModel.AspNetUserId),
                new Claim(JwtRegisteredClaimNames.Email, jWTGenerateTokenModel.AspNetEmail),
                new Claim("studentId", jWTGenerateTokenModel.StudentId.ToString())
            };

            foreach (var claim in jWTGenerateTokenModel.Roles)
            {
                claims.Add(new Claim("claims", claim));
            }

            foreach (var claim in jWTGenerateTokenModel.Classes)
            {
                claims.Add(new Claim("classes", claim.ToString()));
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                null,
                DateTime.UtcNow.Add(_jwtOptions.TokenLifeTime),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public string RefreshToken(List<Claim> tokenClaims)
        {

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                tokenClaims,
                null,
                DateTime.UtcNow.Add(_jwtOptions.TokenLifeTime),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }

        public RefreshTokenDTO GenerateRefreshToken(string userId)
        {
            string token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            string tokenHash = GenerateSha256Hash(token);
            RefreshTokenDTO refreshTokenDTO = new()
            {
                Token = token,
                TokenHash = tokenHash,
                AspNetUserId = userId
            };

            return refreshTokenDTO;
        }

        private static string GenerateSha256Hash(string token)
        {
            var tokenHashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            string tokenBase64 = Convert.ToBase64String(tokenHashBytes);

            return tokenBase64;
        }
    }

    public class RefreshTokenDTO
    {
        public string AspNetUserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string TokenHash { get; set; } = string.Empty;
    }
}
