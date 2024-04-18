using AnswerCube.UI.MVC.Controllers.DTO_s;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class ContactinfoController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    //Mailservice _mailService = new Mailservice();
    
    public ContactinfoController(ILogger<HomeController> logger)
    {
        _logger = logger;
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
            //var registrationLink = "https://localhost:5001/Identity/Account/Register";
            //var companyName = "AnswerCube";
            //var companyMail = "AnswerCube@gmail.com";
            //_mailService.SendEmail(contactInfo.Email, "Contact info " + companyName, "Klik <a href='https://localhost:5001/Identity/Account/Register'>hier</a> om je te registreren en op de forums te kunnen.",companyMail,companyName);
            _logger.LogInformation("Contact info received: {0} Email: {1} Name: {2}", contactInfo, contactInfo.Email,
                contactInfo.Name);
            return RedirectToAction("Index", "Home");
        }

        _logger.LogWarning("Invalid contact info received: {0} Email: {1} Name: {2}", contactInfo, contactInfo.Email,
            contactInfo.Name);
        return View("ContactInfo");
    }
}