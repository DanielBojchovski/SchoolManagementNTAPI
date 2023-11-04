using SchoolManagementNTAPI.Principal.Interfaces;
using SchoolManagementNTAPI.Principal.Services;
using SchoolManagementNTAPI.School.Interfaces;
using SchoolManagementNTAPI.School.Services;

namespace SchoolManagementNTAPI.AppStartup
{
    public static class DependencyInjectionBuilder
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ISchoolService, SchoolService>();

            services.AddScoped<IPrincipalService, PrincipalService>();

            return services;
        }
    }
}
