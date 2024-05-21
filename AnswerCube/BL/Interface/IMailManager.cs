namespace AnswerCube.BL;

public interface IMailManager
{
    Task SendExistingEmail(string email, string organizationName);
    Task SendNewEmail(string email, string organizationName);
    Task SendConfirmationEmail(string email, string userId, string code, string returnUrl);
}