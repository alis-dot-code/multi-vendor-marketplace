namespace MarketNest.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string htmlBody);
    Task SendTemplatedEmailAsync(string to, string templateId, Dictionary<string, string> templateData);
}
