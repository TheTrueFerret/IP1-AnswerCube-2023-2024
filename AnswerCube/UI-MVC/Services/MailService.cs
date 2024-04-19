using System.Net;
using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AnswerCube.UI.MVC.Services;

public class MailService : IEmailSender
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailService> _logger;

    public MailService(IConfiguration configuration, ILogger<MailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation(Environment.GetEnvironmentVariable("Smtpfrom"));
        _logger.LogInformation(Environment.GetEnvironmentVariable("SmtpServer"));
        _logger.LogInformation(Environment.GetEnvironmentVariable("SmtpPort"));
        _logger.LogInformation(Environment.GetEnvironmentVariable("SmtpUsername"));
        _logger.LogInformation(Environment.GetEnvironmentVariable("SmtpPassword"));
        return Execute("answercubeintegratie@gmail.com", email, subject, htmlMessage);
    }

    public Task SendEmailToCompany(string companyMail, string subject, string htmlMessage)
    {
        //TODO: Implement SendEmailToCompany
        throw new NotImplementedException();
    }

    private async Task Execute(string fromEmail, string toEmail, string subject, string htmlMessage)
    {
        var apiKey = Environment.GetEnvironmentVariable("SENDGRIDAPIKEY");
        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage
        {
            From = new EmailAddress(fromEmail, "AnswerCube"),
            Subject = subject,
            PlainTextContent = htmlMessage,
            HtmlContent = htmlMessage
        };
        msg.AddTo(new EmailAddress(toEmail));
        msg.SetClickTracking(false, false);
        await client.SendEmailAsync(msg);
    }
}