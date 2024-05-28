using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.DAL.EF;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlidesController : BaseController
{
    private readonly IFlowManager _flowManager;
    private readonly ILogger<FlowController> _logger;
    private readonly UnitOfWork _uow;
    
    public SlidesController(IFlowManager flowManager, ILogger<FlowController> logger, UnitOfWork uow)
    {
        _flowManager = flowManager;
        _logger = logger;
        _uow = uow;
    }
    
    
    public IActionResult SlidesDetails(int slideId)
    {
        Slide slide = _flowManager.GetSlideById(slideId);
        return View(slide);
    }
    
    
    public IActionResult UpdateSlide(string text, List<string>? answersList, int slide_id, int slideListId)
    {
        if (ModelState.IsValid)
        {
            _uow.BeginTransaction();
            _flowManager.UpdateSlide(text, answersList, slide_id);
            _uow.Commit();
            ViewBag.SlideListId = slideListId;
            return RedirectToAction("SlideListDetails", "SlideList",new { slidelistId = slideListId });
        }

        return RedirectToAction("EditSlide", new { slideId = slide_id });
    }
 

    public IActionResult EditSlide(int slideId)
    {
        Slide slide = _flowManager.GetSlideById(slideId);
        return View(slide);
    }
    
    
}