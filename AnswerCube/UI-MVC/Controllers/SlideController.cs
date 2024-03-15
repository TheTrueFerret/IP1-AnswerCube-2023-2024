using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers.DTO_s;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideController : Controller
{
    private readonly IManager _manager;
    private readonly ILogger<SlideController> _logger;

    public SlideController(IManager manager, ILogger<SlideController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult InfoSlide()
    {
        List<Info> infos = _manager.GetInfoSlides();
        InfoDto info = new InfoDto(infos[0]);
        return View(info);
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Index()
    {
        return View();
    }
}