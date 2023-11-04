using SchoolManagementNTAPI.School.Interfaces;
using SchoolManagementNTAPI.School.Services;

namespace SchoolManagementNTAPI.AppStartup
{
    public static class DependencyInjectionBuilder
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ISchoolService, SchoolService>();

            return services;
        }
    }
}
