using System.Net;
using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity.UI.Services;

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

    private Task Execute(string fromEmail, string toEmail, string subject, string htmlMessage)
    {
        _logger.LogInformation("OAuth authentication started...");
        // OAuth2 authentication
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            new ClientSecrets
            {
                ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID"),
                ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET")
            },
            new[] { "https://mail.google.com/" }, // Scope for accessing Gmail
            Environment.GetEnvironmentVariable("SMTPUSERNAME"), // User identifier (can be any string)
            CancellationToken.None
        ).Result;
        
        _logger.LogInformation("OAuth authentication completed...");
        _logger.LogInformation(credential.ToString());

        var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("", credential.Token.AccessToken),
            EnableSsl = true
        };
        var mailMessage = new MailMessage(fromEmail, toEmail, subject, htmlMessage)
        {
            IsBodyHtml = true
        };
        smtpClient.Send(mailMessage);
        return Task.CompletedTask;
    }
}