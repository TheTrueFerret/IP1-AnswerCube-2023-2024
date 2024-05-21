using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwtService;
    private int _counter = 0;
    
    
    public CircularFlowController(ILogger<HomeController> logger, IManager manager, IHttpContextAccessor httpContextAccessor, JwtService jwtService)
    {
        _logger = logger;
        _manager = manager;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;

    }
    
    public IActionResult CircularFlow(int? id)
    {
        bool installationUpdated = false;
        int installationId;
        if (id == null)
        {
            string token = Request.Cookies["jwtToken"];
            installationId = _jwtService.GetInstallationIdFromToken(token);
        }
        else
        {
            installationId = (int)id;
            installationUpdated = _manager.UpdateInstallation(installationId);
        }
        if (installationUpdated)
        {
            Slide slide = _manager.GetActiveSlideByInstallationId(installationId);
            string actionName = slide.SlideType.ToString();
            return RedirectToAction(actionName);
        }
        return RedirectToAction("Subthemes");
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
    public IActionResult Subthemes()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        Flow flow = _manager.GetFlowByInstallationId(installationId);
        FlowModel flowModel = new FlowModel
        {
            Id = flow.Id,
            Name = flow.Name,
            Description = flow.Description,
            CircularFlow = flow.CircularFlow,
            SlideLists = flow.SlideLists
        };
        return View("/Views/Slides/Subthemes.cshtml", flowModel);
    }
    
    
    [HttpGet]
    public IActionResult GetNextSlide()
    {
        // Retrieve installation ID from token
        string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        Slide slide = _manager.GetActiveSlideByInstallationId(installationId);
        return new JsonResult(slide);
    }
    
    [HttpGet]
    public IActionResult UpdatePage()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        bool installationUpdated = _manager.UpdateInstallation(installationId);
        string url;
        if (installationUpdated)
        {
            Slide slide = _manager.GetActiveSlideByInstallationId(installationId);
            string actionName = slide.SlideType.ToString();
            url = Url.Action(actionName);
            return Json(new { url });
        }
        url = Url.Action("Subthemes");
        return Json(new { url });
    }
    
    
    [HttpPost]
    public IActionResult ChooseSlideList([FromBody] SlideListDto slideListDto)
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        bool isSlideListUpdated = _manager.AddSlideListToInstallation(slideListDto.Id, installationId);
        if (isSlideListUpdated)
        {
            _manager.UpdateInstallation(installationId);
            Slide slide = _manager.GetActiveSlideByInstallationId(installationId);
            string actionName = slide.SlideType.ToString();
            string url = Url.Action(actionName);
            return Json(new { url }); 
            //return UpdatePage();
        }
        return Error();
    }
    
    
    [HttpPost]
    public IActionResult PostAnswer([FromBody] AnswerModel answer)
    {
        // Retrieve installation ID from token
        string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        int installationId = _jwtService.GetInstallationIdFromToken(token);

        Session? session = _manager.GetSessionByInstallationIdAndCubeId(installationId, answer.CubeId);
        if (session == null)
        {
            Session newSession = new Session()
            {
                CubeId = answer.CubeId
            };
            session = _manager.AddNewSessionWithInstallationId(newSession, installationId);
        }
        
        Slide slide = _manager.GetActiveSlideByInstallationId(installationId);
        List<string> answerText = answer.Answer;
        if (_manager.AddAnswer(answerText, slide.Id, session))
        {
            return UpdatePage();
        }
        return new JsonResult(new BadRequestResult());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}