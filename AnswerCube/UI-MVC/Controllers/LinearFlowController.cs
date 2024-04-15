using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AnswerCube.UI.MVC.Controllers;

public class LinearFlowController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    private FlowModel _flowModel = new FlowModel();
   
    
    public LinearFlowController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    
    public IActionResult LinearFlow()
    {
        if (!_flowModel.PartialPages.ContainsKey(_flowModel.CurrentCondition))
        {
            _flowModel.CurrentCondition = SlideType.StartSlide; // Set a default condition if necessary
        }
        return View(_flowModel);
    }
}