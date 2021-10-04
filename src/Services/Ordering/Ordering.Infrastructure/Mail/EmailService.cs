using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Applications.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService :IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _options;

        public EmailService(IOptions<EmailSettings> options,ILogger<EmailService> logger)
        {
            _logger = logger;
            _options = options.Value;
        }
        public async Task<bool> SendEmail(Email email)
        {
            var client = new SendGridClient(_options.ApiKey);
            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;
            var message = MailHelper.CreateSingleEmail(new EmailAddress(_options.FromAddress,_options.FromName), to, subject, emailBody,emailBody);
            var response = await client.SendEmailAsync(message);

            _logger.LogInformation("Email sent");
            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                return true;

            _logger.LogInformation("Error Sending Mail");
            return false;
        }
    }
}
