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
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailService> _logger;
    private readonly IUrlHelper _urlHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MailService(IConfiguration configuration, ILogger<MailService> logger, IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _logger = logger;
        _urlHelper = urlHelperFactory.GetUrlHelper(actionAccessor.ActionContext);
        _httpContextAccessor = httpContextAccessor;
    }

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        string textmsg = "";
        if (htmlMessage.Equals("newmail"))
        {
            // Generate registration link with the email as a query parameter
            var registerUrl = _urlHelper.Page(
                "/Account/Register",
                pageHandler: null,
                values: new { area = "Identity", email = email },
                protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
            textmsg = File.ReadAllText(@"Services\MailTemplates\NewEmail.txt");
            textmsg = textmsg.Replace("\\n", "\n")
                .Replace("\\\"", "\"")
                .Replace("{registerUrl}", registerUrl);
        }
        else if (htmlMessage.Equals("existingmail"))
        {
            // Generate login link with mail
            var loginUrl = _urlHelper.Page(
                "/Account/Login",
                pageHandler: null,
                values: new { area = "Identity", email = email },
                protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
            textmsg = File.ReadAllText(@"Services\MailTemplates\ExistingEmail.txt");
            textmsg = textmsg.Replace("\\n", "\n")
                .Replace("\\\"", "\"")
                .Replace("{loginUrl}", loginUrl);
        }
        else
        {
            var registerText = HtmlEncoder.Default.Encode(htmlMessage);
            textmsg = File.ReadAllText(@"Services\MailTemplates\RegisterMail.txt");
            textmsg =  textmsg.Replace("{registerText}", registerText);
        }

        return Execute("answercubeintegratie@gmail.com", email, subject, textmsg);
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