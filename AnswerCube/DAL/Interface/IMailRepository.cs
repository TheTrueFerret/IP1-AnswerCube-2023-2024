namespace AnswerCube.DAL;

public interface IMailRepository
{
    Task SendExistingEmail(string email, string organizationName);
    Task SendNewEmail(string email, string organizationName);
    Task SendConfirmationEmail(string email, string userId, string code, string returnUrl);
    Task SendExistingSupervisorEmail(string email, string organizationName);
    Task SendNewSupervisorEmail(string email, string organizationName);
}