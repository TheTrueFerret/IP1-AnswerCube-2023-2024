using AnswerCube.DAL;
using Domain;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AnswerCube.BL;

public class EmailManager : IEmailManager
{
    private readonly IMailRepository _mailRepository;

    public EmailManager(IMailRepository mailRepository)
    {
        _mailRepository = mailRepository;
    }

    public Task SendExistingEmail(string email, string organizationName)
    {
        return _mailRepository.SendExistingEmail(email, organizationName);
    }

    public Task SendNewEmail(string email, string organizationName)
    {
        return _mailRepository.SendNewEmail(email, organizationName);
    }

    public Task SendConfirmationEmail(string email, string userId, string code, string returnUrl)
    {
        return _mailRepository.SendConfirmationEmail(email, userId, code, returnUrl);
    }
}