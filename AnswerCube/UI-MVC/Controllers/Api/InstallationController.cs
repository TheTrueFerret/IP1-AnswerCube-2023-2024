using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace AnswerCube.UI.MVC.Controllers;

public class InstallationController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    private int slideCount = 0;
    private int slideListId;
    
    public InstallationController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    //this file gets all of the date on the active flow on this installation
    
    [HttpGet]
    public ActionResult<SlideListDto> initialiseSlideList()
    {
        slideCount = 0;
        List<SlideList> allSlides = _manager.GetSlideLists();
        SlideListDto slideListDto = new SlideListDto(allSlides[1]);
        slideListId = slideListDto.Id;
        return slideListDto;
    }
    
    [HttpGet]
    public ActionResult<SlideDto> NextSlide()
    {
        SlideList slideList = _manager.GetSlideListById(slideListId);
        SlideDto slideDto = new SlideDto(slideList.Slides.ElementAt(slideCount));
        slideCount++;
        return slideDto;
    }
    
}