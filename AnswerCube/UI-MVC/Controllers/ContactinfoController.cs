using AnswerCube.UI.MVC.Controllers.DTO_s;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class ContactinfoController : BaseController
{
    private readonly ILogger<ContactinfoController> _logger;
    private readonly IEmailSender _mailService;

    public ContactinfoController(ILogger<ContactinfoController> logger, IEmailSender mailService)
    {
        _logger = logger;
        _mailService = mailService;
    }

    public IActionResult ContactInfo()
    {
        return View();
    }

    public IActionResult PostContactInfo(ContactInfoDto contactInfo)
    {
        if (ModelState.IsValid)
        {
            //TODO:Mail sturen
            var registerUrl = Url.Page(
                "/Account/Register",
                pageHandler: null,
                values: new { area = "Identity" },
                protocol: Request.Scheme);
            _mailService.SendEmailAsync(contactInfo.Email, "Contact info AnswerCube",
                $"<!DOCTYPE html> <html lang='en'><head>    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Register yourself!</title>\n    <style>\n        /* Styles for the email template */\n        body {{\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            text-align: center;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 20px auto;\n            background-color: #fff;\n            padding: 20px;\n            border-radius: 8px;\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n        }}\n\n        h1 {{\n            color: #333;\n        }}\n\n        p {{\n            color: #666;\n            margin-bottom: 20px;\n        }}\n\n        .btn {{\n            display: inline-block;\n            padding: 10px 20px;\n            background-color: #007bff;\n            color: #fff;\n            text-decoration: none;\n            border-radius: 5px;\n            transition: background-color 0.3s;\n        }}\n\n        .btn:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Register yourself!</h1>\n        <p>Thank you for leaving your contact info, Please register by using the button below!</p>\n        <a href=\"{registerUrl}\" class=\"btn\">Register Here</a>\n    </div>\n</body>\n</html>\n");

            _logger.LogInformation("Contact info received: {0} Email: {1} Name: {2}", contactInfo, contactInfo.Email,
                contactInfo.Name);
            return RedirectToAction("Index", "Home");
        }

        _logger.LogWarning("Invalid contact info received: {0} Email: {1} Name: {2}", contactInfo, contactInfo.Email,
            contactInfo.Name);
        return View("ContactInfo");
    }
}