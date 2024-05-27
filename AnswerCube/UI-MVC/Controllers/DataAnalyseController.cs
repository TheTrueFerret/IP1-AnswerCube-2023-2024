using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnswerCube.UI.MVC.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
public class DataAnalyseController : BaseController
{
    private readonly IAnswerManager _answerManager;
    private readonly IFlowManager _flowManager;
    private readonly ILogger<DataAnalyseController> _logger;
    
    public DataAnalyseController(IAnswerManager answerManager, IFlowManager flowManager, ILogger<DataAnalyseController> logger)
    {
        _answerManager = answerManager;
        _flowManager = flowManager;
        _logger = logger;
    }

    [HttpGet("Answers")]
    public ActionResult<List<Answer>> GetAnswers()
    {
        var answers = _answerManager.GetAnswers();
        return answers;
    }
    
    [HttpGet("Slides")]
    public ActionResult<List<Slide>> GetSlides()
    {
        var slides = _answerManager.GetSlides();
        return slides;
    }
    
    [HttpGet("Sessions")]
    public ActionResult<List<Session>> GetSessions()
    {
        var sessions = _answerManager.GetSessions();
        return sessions;
    }

    [HttpGet("SlideById/{id:int}")]
    public Slide GetSlideById(int id)
    {
        return _flowManager.GetSlideById(id);
    }
    
    [HttpGet("AnswersBySessionId/{sessionId:int}")]
    public ActionResult<List<Answer>> GetAnswersBySessionId(int sessionId)
    {
        var answers = _answerManager.GetAnswersBySessionId(sessionId);
        return answers;
    }

    public IActionResult Answers()
    {
        return View();
    }
}