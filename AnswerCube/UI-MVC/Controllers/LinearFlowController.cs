using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnswerCube.UI.MVC.Controllers;

public class LinearFlowController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    
    public LinearFlowController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    public IActionResult LinearFlow()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult MultipleChoice()
    {
        return View("/Views/Slides/MultipleChoice.cshtml");
    }
    
    [HttpGet]
    public IActionResult OpenQuestion()
    {
        return View("/Views/Slides/OpenQuestion.cshtml");
    }
    
    [HttpGet]
    public IActionResult SingleChoice()
    {
        return View("/Views/Slides/SingleChoice.cshtml");
    }
    
    [HttpGet]
    public IActionResult InfoSlide()
    {
        return View("/Views/Slides/InfoSlide.cshtml");
    }
    
    [HttpPost]
    public IActionResult getNextSlide(int slideListId)
    {
        SlideList slideList = _manager.GetSlideList();
        return new JsonResult(slideList);
    }
    
    //this file gets all of the date on the active flow on this installation
    [HttpPost]
    public IActionResult updatePAge(int currentSlideIndex, SlideList slideList)
    {
        Slide slide = _manager.GetSlideFromSlideListByIndex(currentSlideIndex, slideList.Id);
        string actionName = slide.SlideType.ToString();
        string url = Url.Action(actionName);
        return Json(new { url });
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}