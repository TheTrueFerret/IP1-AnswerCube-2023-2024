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

    public InstallationController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    [HttpGet]
    public ActionResult<SlideDto> NextSlide()
    {
        List<Slide> allSlides = _manager.GetListSlides();
        Random rand = new Random();
        SlideDto slideDto = new SlideDto(allSlides[rand.Next(allSlides.Count)]);
        return slideDto;
    }
    
}