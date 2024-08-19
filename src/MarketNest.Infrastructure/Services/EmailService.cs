using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MarketNest.Application.Common.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MarketNest.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly EmailAddress _from;

        public EmailService(IConfiguration config)
        {
            var apiKey = config["SendGridSettings:ApiKey"] ?? config["SendGrid:ApiKey"];
            _client = new SendGridClient(apiKey);
            var fromEmail = config["SendGridSettings:FromEmail"] ?? config["SendGrid:FromEmail"] ?? "no-reply@marketnest.example";
            var fromName = config["SendGridSettings:FromName"] ?? config["SendGrid:FromName"] ?? "MarketNest";
            _from = new EmailAddress(fromEmail, fromName);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string htmlContent)
        {
            var msg = MailHelper.CreateSingleEmail(_from, new EmailAddress(toEmail), subject, null, htmlContent);
            await _client.SendEmailAsync(msg);
        }

        public async Task SendTemplatedEmailAsync(string toEmail, string templateId, Dictionary<string, string> templateData)
        {
            var msg = new SendGridMessage();
            msg.SetFrom(_from);
            msg.AddTo(new EmailAddress(toEmail));
            msg.SetTemplateId(templateId);
            msg.SetTemplateData(templateData);
            await _client.SendEmailAsync(msg);
        }
    }
}
