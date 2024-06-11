using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AnswerCube.DAL.EF;

public class MailRepository : IMailRepository
{
    private readonly IEmailSender _emailSender;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string htmlMessage = "";

    public MailRepository(IEmailSender emailSender, IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionAccessor;
        _httpContextAccessor = httpContextAccessor;
        _emailSender = emailSender;
    }
    
    private IUrlHelper _urlHelper => _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);

    public async Task SendExistingEmail(string email, string organizationName)
    {
        // Generate login link with mail
        var loginUrl = _urlHelper.Page(
            "/Account/Login",
            pageHandler: null,
            values: new { area = "Identity", email = email },
            protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
        htmlMessage = File.ReadAllText(@"Services/MailTemplates/ExistingEmail.txt");
        htmlMessage = htmlMessage.Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("{loginUrl}", loginUrl);

        await _emailSender.SendEmailAsync(email, $"You have been added as a DeelplatformBeheeder to {organizationName}",
            htmlMessage);
    }

    public async Task SendNewEmail(string email, string organizationName)
    {
        // Generate registration link with the email as a query parameter
        var registerUrl = _urlHelper.Page(
            "/Account/Register",
            pageHandler: null,
            values: new { area = "Identity", email = email },
            protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
        htmlMessage = File.ReadAllText(@"Services/MailTemplates/NewEmail.txt");
        htmlMessage = htmlMessage.Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("{registerUrl}", registerUrl);
        await _emailSender.SendEmailAsync(email, $"Register as DeelplatformBeheerder for {organizationName}",
            htmlMessage);
    }

    public async Task SendConfirmationEmail(string email, string userId, string code, string returnUrl)
    {
        var callbackUrl = _urlHelper.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
            protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
        htmlMessage = File.ReadAllText(@"Services/MailTemplates/ConfirmEmail.txt");
        htmlMessage = htmlMessage.Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("{callbackUrl}", callbackUrl);
        await _emailSender.SendEmailAsync(email, "Confirm your email",
            htmlMessage);
    }

    public async Task SendExistingSupervisorEmail(string email, string organizationName)
    {
        // Generate login link with mail
        var loginUrl = _urlHelper.Page(
            "/Account/Login",
            pageHandler: null,
            values: new { area = "Identity", email = email },
            protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
        htmlMessage = File.ReadAllText(@"Services/MailTemplates/ExistingEmail.txt");
        htmlMessage = htmlMessage.Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("{loginUrl}", loginUrl);

        await _emailSender.SendEmailAsync(email, $"You have been added as a Supervisor to {organizationName}",
            htmlMessage);
    }

    public async Task SendNewSupervisorEmail(string email,string organizationName)
    {
        // Generate registration link with the email as a query parameter
        var registerUrl = _urlHelper.Page(
            "/Account/Register",
            pageHandler: null,
            values: new { area = "Identity", email = email },
            protocol: _httpContextAccessor.HttpContext?.Request.Scheme);
        htmlMessage = File.ReadAllText(@"Services/MailTemplates/NewEmail.txt");
        htmlMessage = htmlMessage.Replace("\\n", "\n")
            .Replace("\\\"", "\"")
            .Replace("{registerUrl}", registerUrl);
        await _emailSender.SendEmailAsync(email, $"Register as Supervisor for {organizationName}",
            htmlMessage);
    }
}