using Microsoft.AspNetCore.Http;

namespace SchoolManagementNTAPI.Authentication.Cookies
{
    public class DefaultCookieOptions
    {
        public bool HttpOnly { get; set; }
        public bool CookieSSL { get; set; }
        public int RefreshTokenLifeTimeDays { get; set; }

        public CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = HttpOnly,
                Secure = CookieSSL,
                Expires = DateTimeOffset.UtcNow.AddDays(RefreshTokenLifeTimeDays)
            };
        }
    }
}
