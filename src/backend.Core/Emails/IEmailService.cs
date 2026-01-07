using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.Core.Emails
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string to, string subject, string body, string[]? attachments = null);
        public Task SendEmailsAsync(IEnumerable<string> to, string subject, string body, string[]? attachments = null);
        public Task SendEmailWithTemplateAsync(string to, string subject, EmailTemplate template, string body, string[]? attachments = null);
        public Task SendEmailsWithTemplateAsync(IEnumerable<string> to, string subject, EmailTemplate template, string body, string[]? attachments = null);

    }
}
