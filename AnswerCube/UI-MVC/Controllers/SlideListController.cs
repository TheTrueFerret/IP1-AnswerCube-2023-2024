using AnswerCube.BL;
using AnswerCube.DAL.EF;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideListController : BaseController
{
    private readonly IFlowManager _flowManager;
    private readonly ILogger<FlowController> _logger;
    private readonly UnitOfWork _uow;

    public SlideListController(IFlowManager flowManager, ILogger<FlowController> logger, UnitOfWork uow)
    {
        _flowManager = flowManager;
        _logger = logger;
        _uow = uow;
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
            _uow.BeginTransaction();
            _flowManager.UpdateSlideList(title, description, slideListId);
            _uow.Commit();
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