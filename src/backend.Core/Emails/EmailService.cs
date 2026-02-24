using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace backend.Core.Emails
{
    public class EmailService(IConfiguration configuration) : IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body, string[]? attachments = null)
        {
            return SendEmailsAsync([to], subject, body, attachments);
        }

        public async Task SendEmailsAsync(IEnumerable<string> to, string subject, string body, string[]? attachments = null)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(configuration["Email:FromAddress"] ?? throw new InvalidOperationException("Email:FromAddress is not configured"), configuration["Email:FromName"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var recipient in to)
            {
                mailMessage.To.Add(recipient);
            }
            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    mailMessage.Attachments.Add(new Attachment(attachment));
                }
            }
            await SmtpClient.SendMailAsync(mailMessage);
        }

        public Task SendEmailsWithTemplateAsync(IEnumerable<string> to, string subject, EmailTemplate template, string body, string[]? attachments = null)
        {
            body = BuildWithTemplate(template, body);
            return SendEmailsAsync(to, subject, body, attachments);
        }

        public Task SendEmailWithTemplateAsync(string to, string subject, EmailTemplate template, string body, string[]? attachments = null)
        {
            return SendEmailsWithTemplateAsync([to], subject, template, body, attachments);
        }

        private SmtpClient SmtpClient
        {
            get
            {
                var smtpClient = new SmtpClient(configuration["Email:SmtpServer"])
                {
                    Port = int.Parse(configuration["Email:SmtpPort"] ?? throw new InvalidOperationException("Email:SmtpPort is not configured")),
                    Credentials = new System.Net.NetworkCredential(configuration["Email:Username"], configuration["Email:Password"]),
                    EnableSsl = bool.Parse(configuration["Email:EnableSsl"] ?? throw new InvalidOperationException("Email:EnableSsl is not configured"))
                };
                return smtpClient;
            }
        }

        private static string BuildWithTemplate(EmailTemplate template, string body)
        {
            return template switch
            {
                EmailTemplate.Default => $"<h1>Example App</h1>" +
                $"<br />{body}",
                EmailTemplate.Auth => $"<h1>Auth</h1>" +
                $"<br />{body}",
                EmailTemplate.Suspicious_Behavior => $"<h1>We've noticed something</h1>" +
                $"<br />{body}",
                _ => $"{body}"
            };
        }
    }
}
