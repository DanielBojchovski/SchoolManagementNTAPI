using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SchoolManagementNTAPI.Authentication.Cookies
{
    public class CookieOptionsSetup : IConfigureOptions<DefaultCookieOptions>
    {
        private const string SectionName = "DefaultCookieOptions";
        private readonly IConfiguration _configuration;

        public CookieOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(DefaultCookieOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
