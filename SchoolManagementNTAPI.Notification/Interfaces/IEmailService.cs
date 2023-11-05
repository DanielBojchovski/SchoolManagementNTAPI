using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Notification.Requests;
using SchoolManagementNTAPI.Notification.Responses;

namespace SchoolManagementNTAPI.Notification.Interfaces
{
    public interface IEmailService
    {
        public Task<SendEmailResponse> SendEmail(SendEmailRequest request);
    }
}
