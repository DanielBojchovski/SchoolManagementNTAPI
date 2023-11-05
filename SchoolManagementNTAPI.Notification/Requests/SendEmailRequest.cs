﻿namespace SchoolManagementNTAPI.Notification.Requests
{
    public class SendEmailRequest
    {
        public List<string> EmailTo { get; set; } = new();
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
    }
}
