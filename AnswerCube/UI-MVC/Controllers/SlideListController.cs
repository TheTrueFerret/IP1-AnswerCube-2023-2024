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


    [HttpGet]
    public IActionResult SlideListDetails(int slidelistId)
    {
        SlideList slideList = _manager.GetSlideListWithFlowById(slidelistId);
        foreach (var cSlides in slideList.ConnectedSlides)
        {
            _logger.LogInformation(cSlides.Slide.ToString());
        }

        return View(slideList);
    }

    public IActionResult EditSlideList(string title, string description, int slideListId)
    {
        if (ModelState.IsValid)
        {
            _manager.UpdateSlideList(title, description, slideListId);
            return RedirectToAction("SlideListDetails", new { slidelistId = slideListId });
        }

        return RedirectToAction("EditSlideListView", new { slidelistId = slideListId });
    }


    public IActionResult EditSlideListView(int slidelistId)
    {
        SlideList slideList = _manager.GetSlideListWithFlowById(slidelistId);
        return View(slideList);
    }
}