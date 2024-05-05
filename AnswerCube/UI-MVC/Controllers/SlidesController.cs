using AnswerCube.BL;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlidesController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;

    public SlidesController(IManager manager, ILogger<FlowController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    
    
    public IActionResult SlidesDetails(int slideId)
    {
        Slide slide = _manager.GetSlideById(slideId);
        return View(slide);
    }
    
    
    
}