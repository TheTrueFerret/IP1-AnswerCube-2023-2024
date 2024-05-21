using AnswerCube.BL;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideListController : BaseController
{
    private readonly IFlowManager _flowManager;
    private readonly ILogger<FlowController> _logger;

    public SlideListController(IFlowManager flowManager, ILogger<FlowController> logger)
    {
        _flowManager = flowManager;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult SlideListDetails(int slidelistId)
    {
        SlideList slideList = _flowManager.GetSlideListWithFlowById(slidelistId);
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
            _flowManager.UpdateSlideList(title, description, slideListId);
            return RedirectToAction("SlideListDetails", new { slidelistId = slideListId });
        }

        return RedirectToAction("EditSlideListView", new { slidelistId = slideListId });
    }


    public IActionResult EditSlideListView(int slidelistId)
    {
        SlideList slideList = _flowManager.GetSlideListWithFlowById(slidelistId);
        return View(slideList);
    }
}