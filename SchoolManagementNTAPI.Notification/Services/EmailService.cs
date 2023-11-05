using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using MimeKit;
using SchoolManagementNTAPI.Authentication.JWT;
using SchoolManagementNTAPI.Notification.Interfaces;
using SchoolManagementNTAPI.Notification.Options;
using SchoolManagementNTAPI.Notification.Requests;
using SchoolManagementNTAPI.Notification.Responses;
using MailKit.Net.Smtp;

namespace SchoolManagementNTAPI.Notification.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailOptions _mailOptions;

        public EmailService(IOptions<MailOptions> options)
        {
            _mailOptions = options.Value;
        }

        public async Task<SendEmailResponse> SendEmail(SendEmailRequest request)
        {
            SendEmailResponse response = new();
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailOptions.EmailUserName, _mailOptions.EmailUserName));
            message.Subject = request.EmailSubject;
            message.Body = new TextPart(TextFormat.Html) { Text = request.EmailBody };
            using var client = new SmtpClient();

            try
            {
                client.Connect(_mailOptions.EmailHost, _mailOptions.Port, (SecureSocketOptions)_mailOptions.SSL);
                client.Authenticate(_mailOptions.EmailUserName, _mailOptions.EmailPassword);
            }
            catch
            {
                response.Success = false;
                client.Disconnect(true);
                return response;
            }

            foreach (var email in request.EmailTo)
            {
                try
                {
                    message.To.Clear();
                    message.To.Add(MailboxAddress.Parse(email));
                    await client.SendAsync(message);
                    response.EmailsWhichRecieveMail.Add(email);
                }
                catch (Exception)
                {
                    response.EmailsWhichDidntRecieveMail.Add(email);
                }
            }

            client.Disconnect(true);

            response.Success = response.EmailsWhichDidntRecieveMail.Count == 0;

            return response;
        }
    }
}
