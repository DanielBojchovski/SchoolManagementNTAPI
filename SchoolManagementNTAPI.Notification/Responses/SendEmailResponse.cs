namespace SchoolManagementNTAPI.Notification.Responses
{
    public class SendEmailResponse
    {
        public bool Success { get; set; }
        public List<string> EmailsWhichRecieveMail { get; set; } = new();
        public List<string> EmailsWhichDidntRecieveMail { get; set; } = new();
    }
}
