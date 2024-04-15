using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class CircularFlowController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    private readonly FlowModel _flowModel;
    
    
    public CircularFlowController(ILogger<HomeController> logger, IManager manager, FlowModel flowModel)
    {
        _logger = logger;
        _manager = manager;
        _flowModel = flowModel;
    }
    
    public IActionResult CircularFlow()
    {
        return View(_flowModel);
    }
    
    [HttpPost]
    public IActionResult InitializeFlow(int slideListId)
    {
        SlideList slideList = _manager.GetSlideList();
        return new JsonResult(slideList);
    }
    
    //this file gets all of the date on the active flow on this installation
    [HttpPost]
    public IActionResult NextSlide(int currentSlideIndex, int slideListId)
    {
        Slide slide = _manager.GetSlideFromSlideListByIndex(currentSlideIndex, slideListId);
        PartialViewResult result = SetCurrentSlide(slide);
        return result;
    }
    
    private string GetPartialViewName(SlideType condition)
    {
        return _flowModel.PartialPages.ContainsKey(condition) ? _flowModel.PartialPages[condition] : "Slide/StartSlide"; // Provide a default partial view if necessary
    }
    
    public PartialViewResult SetCurrentSlide(Slide slide)
    {
        _logger.LogInformation(slide.SlideType + "-" +  slide.Id);
        
        // Check if the received slide type exists in PartialPages
        if (_flowModel.PartialPages.ContainsKey(slide.SlideType))
        {
            _flowModel.CurrentCondition = slide.SlideType;
            string partialViewName = GetPartialViewName(_flowModel.CurrentCondition);

            return PartialView(partialViewName); // Return the partial view
        }
        else
        {
            return null;
        }
    }

    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}