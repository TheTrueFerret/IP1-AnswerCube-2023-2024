using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideController : BaseController
{
    
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
    
    [HttpGet]
    public IActionResult RangeQuestion()
    {
        return View("/Views/Slides/RangeQuestion.cshtml");
    }
}