using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Models.Dto;
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
    private readonly IAnswerManager _answerManager;
    private readonly IFlowManager _flowManager;
    private readonly IInstallationManager _installationManager;
    private readonly JwtService _jwtService;
    private readonly int _counter = 0;
    
    public CircularFlowController(ILogger<HomeController> logger, IAnswerManager answerManager, IFlowManager flowManager, IInstallationManager installationManager, JwtService jwtService)
    {
        _logger = logger;
        _answerManager = answerManager;
        _flowManager = flowManager;
        _installationManager = installationManager;
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
            installationUpdated = _installationManager.UpdateInstallation(installationId);
        }
        if (installationUpdated)
        {
            Slide slide = _installationManager.GetActiveSlideByInstallationId(installationId);
            string actionName = slide.SlideType.ToString();
            return RedirectToAction(actionName);
        }
        return RedirectToAction("Subthemes");
    }
    
    [HttpGet]
    public IActionResult MultipleChoice()
    {
        return View("/Views/Slides/MultipleChoice.cshtml", GetNextSlide());
    }
    
    [HttpGet]
    public IActionResult OpenQuestion()
    {
        return View("/Views/Slides/OpenQuestion.cshtml", GetNextSlide());
    }
    
    [HttpGet]
    public IActionResult SingleChoice()
    {
        return View("/Views/Slides/SingleChoice.cshtml", GetNextSlide());
    }
    
    [HttpGet]
    public IActionResult InfoSlide()
    {
        return View("/Views/Slides/InfoSlide.cshtml", GetNextSlide());
    }
    
    [HttpGet]
    public IActionResult RangeQuestion()
    {
        return View("/Views/Slides/RangeQuestion.cshtml", GetNextSlide());
    }
    
    [HttpGet]
    public IActionResult Subthemes()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        Flow flow = _flowManager.GetFlowByInstallationId(installationId);
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
    
    
    public SlideCompositeModel GetNextSlide()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        Slide slide = _installationManager.GetActiveSlideByInstallationId(installationId);
        SlideList slideList = _flowManager.GetSlideListByInstallationId(installationId);
        SlideModel slideModel = new SlideModel
        {
            Id = slide.Id,
            Text = slide.Text,
            SlideType = slide.SlideType,
            AnswerList = slide.AnswerList,
            MediaUrl = slide.MediaUrl,
            SlideList = slideList
        };
        
        Flow flow = _flowManager.GetFlowByInstallationId(installationId);
        
        SlideCompositeModel slideCompositeModel = new SlideCompositeModel
        {
            SlideModel = slideModel
        };

        if (flow.CircularFlow)
        {
            CircularFlowModel circularFlowModel = new CircularFlowModel
            {
                SubTheme = slideList.SubTheme
            };
            slideCompositeModel.CircularFlowModel = circularFlowModel;
        }
        else
        {
            LinearFlowModel linearFlowModel = new LinearFlowModel()
            {
                SubTheme = slideList.SubTheme
            };
            slideCompositeModel.LinearFlowModel = linearFlowModel;
        }
        int forumId = _installationManager.GetForumIdByInstallationId(installationId);
        if (forumId != 0)
        {
            slideCompositeModel.forumId = forumId;
        }
        else
        {
            slideCompositeModel.forumId = 0;
        }
        return slideCompositeModel;
    }
    
    [HttpGet]
    public IActionResult UpdatePage()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        bool installationUpdated = _installationManager.UpdateInstallation(installationId);
        string url;
        if (installationUpdated)
        {
            Slide slide = _installationManager.GetActiveSlideByInstallationId(installationId);
            string actionName = slide.SlideType.ToString();
            url = Url.Action(actionName);
            return Json(new { url });
        }
        url = Url.Action("LeaveContactInfoQrCode");
        return Json(new { url });
    }
    
    
    [HttpPost]
    public IActionResult ChooseSlideList([FromBody] SlideListDto slideListDto)
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        bool isSlideListUpdated = _installationManager.AddSlideListToInstallation(slideListDto.Id, installationId);
        if (isSlideListUpdated)
        {
            return UpdatePage();
        }
        return Error();
    }
    
    
    [HttpPost]
    public IActionResult PostAnswer([FromBody] AnswersDto answers)
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        bool[] allAnswersAdded = new bool[answers.Answers.Count];
        
        Slide slide = _installationManager.GetActiveSlideByInstallationId(installationId);
        

        for (int i = 0; i < answers.Answers.Count; i++)
        {
            Session? session = _installationManager.GetActiveSessionByInstallationIdAndCubeId(installationId, answers.Answers[i].CubeId);
            if (session == null)
            {
                Session newSession = new Session()
                {
                    CubeId = answers.Answers[i].CubeId
                };
                session = _installationManager.AddNewSessionWithInstallationId(newSession, installationId);
            }
            List<string> answerText = answers.Answers[i].Answer;
            if (_answerManager.AddAnswer(answerText, slide.Id, session))
            {
                allAnswersAdded[i] = true;
            }
            else
            {
                allAnswersAdded[i] = false;
            }
        }
        
        if (allAnswersAdded.All(answer => answer))
        {
            return UpdatePage();
        }
        
        return new JsonResult(new BadRequestResult());
    }
    
    [HttpGet]
    public IActionResult GetActiveSessionsFromInstallation()
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        List<Session> sessions = _installationManager.GetActiveSessionsByInstallationId(installationId);
        if (sessions.Count != 0)
        {
            int[] CubeIds = new int[sessions.Count];
            for (int i = 0; i < sessions.Count; i++)
            {
                CubeIds[i] = sessions[i].CubeId;
            }
            return new JsonResult(CubeIds);
        }
        else
        {
            return Empty;
        }
    }
    
    
    [HttpPost]
    public IActionResult EndSession([FromBody] int cubeId)
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        Session? session = _installationManager.GetActiveSessionByInstallationIdAndCubeId(installationId, cubeId);
        _installationManager.EndSessionByInstallationIdAndCubeId(installationId, cubeId);
        if (session == null)
        {
            return Error();
        }
        return Ok();
    }
    
    
    [HttpPost]
    public IActionResult StartSession([FromBody] int cubeId)
    {
        // Retrieve installation ID from token
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        Session? session = _installationManager.GetActiveSessionByInstallationIdAndCubeId(installationId, cubeId);
        
        if (session == null)
        {
            Session newSession = new Session()
            {
                CubeId = cubeId
            };
            _installationManager.AddNewSessionWithInstallationId(newSession, installationId);
        }
        return Ok();
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult LeaveContactInfoQrCode()
    {
        return View("/Views/Contactinfo/LeaveContactInfoQrCode.cshtml");
    }
}