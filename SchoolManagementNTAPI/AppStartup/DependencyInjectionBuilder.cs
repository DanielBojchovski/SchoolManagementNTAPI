using SchoolManagementNTAPI.AppUser.Interfaces;
using SchoolManagementNTAPI.AppUser.Services;
using SchoolManagementNTAPI.Authentication.Interfaces;
using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Authentication.Services;
using SchoolManagementNTAPI.Notification.Interfaces;
using SchoolManagementNTAPI.Notification.Services;
using SchoolManagementNTAPI.Principal.Interfaces;
using SchoolManagementNTAPI.Principal.Services;
using SchoolManagementNTAPI.Professor.Interfaces;
using SchoolManagementNTAPI.Professor.Services;
using SchoolManagementNTAPI.Role.Interfaces;
using SchoolManagementNTAPI.Role.Services;
using SchoolManagementNTAPI.School.Interfaces;
using SchoolManagementNTAPI.School.Services;
using SchoolManagementNTAPI.Student.Interfaces;
using SchoolManagementNTAPI.Student.Services;
using SchoolManagementNTAPI.Subject.Interfaces;
using SchoolManagementNTAPI.Subject.Services;

namespace SchoolManagementNTAPI.AppStartup
{
    public static class DependencyInjectionBuilder
    {
        public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
        {
            services.AddScoped<ISchoolService, SchoolService>();

            services.AddScoped<IPrincipalService, PrincipalService>();

            services.AddScoped<IProfessorService, ProfessorService>();

            services.AddScoped<ISubjectService, SubjectService>();

            services.AddScoped<IStudentService, StudentService>();


            services.AddScoped<IRolesService, RolesService>();

            services.AddScoped<IUserService, UserService>();

            //auth
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddScoped<IEmailService, EmailService>();

            return services;
        }
    }
}
