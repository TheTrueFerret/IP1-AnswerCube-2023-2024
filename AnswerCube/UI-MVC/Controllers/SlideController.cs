using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideController : Controller
{
    private readonly IManager _manager;

    public SlideController(IManager manager)
    {
        _manager = manager;
    }
    
    [HttpGet]
    public IActionResult MultipleChoice()
    {
        var multipleChoice = _manager.GetMultipleChoiceSlides();
        return View(multipleChoice);
    }
    
    [HttpGet]
    public IActionResult OpenQuestion()
    {
        var openQuestion = _manager.GetOpenSlides();
        return View(openQuestion);
    }
    
    [HttpGet]
    public IActionResult SingleChoice()
    {
        var singleChoice = _manager.GetSingleChoiceSlides();
        return View(singleChoice);
    }
    
    [HttpGet]
    public IActionResult InfoSlide()
    {
        return View("/Views/Slide/InfoSlide.cshtml");
    }
}