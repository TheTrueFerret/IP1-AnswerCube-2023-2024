using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AnswerCube.UI.MVC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DataAnalyseController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<DataAnalyseController> _logger;

    public DataAnalyseController(IRepository repository, IManager manager, ILogger<DataAnalyseController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [HttpGet("Answers")]
    public ActionResult<List<Answer>> GetAnswers()
    {
        var answers = _manager.GetAnswers();
        return answers;
    }

    [HttpGet("SlideById/{id:int}")]
    public Slide GetSlideById(int id)
    {
        return _manager.GetSlideById(id);
    }

    public IActionResult Answers()
    {
        return View();
    }
}