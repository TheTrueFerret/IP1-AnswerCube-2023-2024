using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlidesController : BaseController
{
    private readonly IFlowManager _flowManager;
    private readonly ILogger<FlowController> _logger;
    
    public SlidesController(IFlowManager flowManager, ILogger<FlowController> logger)
    {
        _flowManager = flowManager;
        _logger = logger;
    }
    
    
    public IActionResult SlidesDetails(int slideId)
    {
        Slide slide = _flowManager.GetSlideById(slideId);
        return View(slide);
    }
    
    
    public IActionResult UpdateSlide(SlideType slideType, string text, List<string> answersList, int slide_id, int slideListId)
    {
        if (ModelState.IsValid)
        {
            _flowManager.UpdateSlide(slideType, text, answersList, slide_id);
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