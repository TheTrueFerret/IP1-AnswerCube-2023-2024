using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;

namespace AnswerCube.UI.MVC.Services;

public class MailService
{
    private static readonly string ServiceAccountPath = "./ServiceAccount.json";

    public void SendEmail(string to, string subject, string body, String companyMail, String companyName)
    {
        try
        {
            var from = companyMail;
            var serviceAccountCredential = GoogleCredential.FromFile(ServiceAccountPath)
                .CreateScoped(GmailService.Scope.GmailSend)
                .UnderlyingCredential as ServiceAccountCredential;

            var initializer = new BaseClientService.Initializer
            {
                HttpClientInitializer = serviceAccountCredential,
                ApplicationName = companyName,
            };

            var service = new GmailService(initializer);

            var msg = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(from)
            };
            msg.To.Add(new MailAddress(to));
            msg.ReplyToList.Add(msg.From);
            var msgStr = new StringWriter();
            msgStr.WriteLine("Beste,");
            msgStr.WriteLine();
            msgStr.WriteLine(body);
            msgStr.WriteLine();
            msgStr.WriteLine("Met vriendelijke groeten,");
            msgStr.WriteLine(companyName);


            msg.Body = msgStr.ToString();

            var result = service.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "me").Execute();
            Console.WriteLine("Message ID: " + result.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while sending the email: " + ex.Message);
        }
    }

    private static string Base64UrlEncode(string input)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(inputBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");
    }
}