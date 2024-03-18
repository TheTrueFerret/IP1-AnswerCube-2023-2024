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
        return View("/Views/Flow/Slide/MultipleChoice.cshtml");
    }
    
    [HttpGet]
    public IActionResult OpenQuestion()
    {
        return View("/Views/Flow/Slide/OpenQuestion.cshtml");
    }
    
    [HttpGet]
    public IActionResult SingleChoice()
    {
        return View("/Views/Flow/Slide/SingleChoice.cshtml");
    }
    
    [HttpGet]
    public IActionResult InfoSlide()
    {
        return View("/Views/Flow/Slide/InfoSlide.cshtml");
    }
}