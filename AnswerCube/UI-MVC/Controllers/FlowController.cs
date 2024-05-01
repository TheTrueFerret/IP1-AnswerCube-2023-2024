using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class FlowController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;

    public FlowController(IManager manager, ILogger<FlowController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult Flow(string? userId, int? organizationId)
    {
        return View();
    }

    public IActionResult CreateSlide()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddSlide(string slideType, string question, string info, string[] options)
    {
        _logger.LogInformation("Creating a new slide");
        _logger.LogInformation("all the data\n" + "SlideType: " +slideType + "\nQuestion: " + question + "\nInfo: " + info );
        options.ToList().ForEach(option => _logger.LogInformation(option));
        //TODO: Add slide to database
        return RedirectToAction("Flows");
    }
    
    
    public IActionResult Flows()
    {
        return View();
    }


    public IActionResult AddFlowToProject()
    {
        return View();
    }
}