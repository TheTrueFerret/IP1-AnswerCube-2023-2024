using AnswerCube.BL;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideListController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;

    public SlideListController(IManager manager, ILogger<FlowController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    
    
    public IActionResult SlideListDetails(int slideListId)
    {
        SlideList slideList =  _manager.GetSlideListWithFlowById(slideListId);
        return View(slideList);
    }

}