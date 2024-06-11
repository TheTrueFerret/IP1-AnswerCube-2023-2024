using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.Encodings.Web;
using Azure.Core;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AnswerCube.UI.MVC.Services;

public class MailService : IEmailSender
{
    private readonly ILogger<MailService> _logger;

    public MailService(ILogger<MailService> logger)
    {
        _logger = logger;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        return Execute("answercubeintegratie@gmail.com", email, subject, htmlMessage);
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
        await client.SendEmailAsync(msg);
    }
}