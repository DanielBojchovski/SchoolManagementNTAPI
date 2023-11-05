using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolManagementNTAPI.Authentication.JWT
{
    public record DecodedJWT
    {
        [FromHeader]
        public string Authorization { get; init; } = string.Empty;

        private JWTModel? _JwtLocal;
        public JWTModel JWT => _JwtLocal ??= ExctractToken(Authorization);

        private static JWTModel ExctractToken(string token)
        {
            var tokenString = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var tokenData = handler.ReadJwtToken(tokenString);

            return new(tokenData.Payload);
        }
    }
}
