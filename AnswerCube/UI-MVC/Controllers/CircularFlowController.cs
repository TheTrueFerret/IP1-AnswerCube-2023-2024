using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class CircularFlowController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    private readonly FlowModel _flowModel;
    
    
    public CircularFlowController(ILogger<HomeController> logger, IManager manager, FlowModel flowModel)
    {
        _logger = logger;
        _manager = manager;
        _flowModel = flowModel;
    }
    
    public IActionResult CircularFlow()
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
    
    [HttpGet]
    public IActionResult RangeQuestion()
    {
        return View("/Views/Slides/RangeQuestion.cshtml");
    }
    
    [HttpGet]
    public IActionResult InitializeFlow()
    {
        SlideList slideList = _manager.GetSlideList();
        /*SlideListDto slideListDto = new SlideListDto()
        {
            Id = slideList.Id,
            Title = slideList.Title,
            SubTheme = slideList.SubTheme,
            Slides = slideList.Slides,
        };*/

        Boolean installationStarted = _manager.StartInstallation(1, slideList);
        if (installationStarted)
        {
            return new JsonResult(slideList);
        }
        else
        {
            return Error();
        }
    }
    
    [HttpGet]
    public IActionResult GetNextSlide()
    {
        Boolean installationUpdated = _manager.UpdateInstallation(1);
        Slide slide = _manager.GetActiveSlideByInstallationId(1);
        return new JsonResult(slide);
    }
    
    //this file gets all of the date on the active flow on this installation
    [HttpGet]
    public IActionResult UpdatePage()
    {
        Slide slide = _manager.GetActiveSlideByInstallationId(1);
        string actionName = slide.SlideType.ToString();
        string url = Url.Action(actionName);
        return Json(new { url });
    }
    
    
    [HttpPost]
    public IActionResult PostAnswer([FromBody] AnswerModel answer)
    {
        Slide slide = _manager.GetActiveSlideByInstallationId(1);
        List<string> answerText = answer.Answer;
        if (_manager.AddAnswer(answerText, slide.Id))
        {
            return new JsonResult(new OkResult());
        }
        else
        {
            return new JsonResult(new BadRequestResult());
        }
    }

    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}