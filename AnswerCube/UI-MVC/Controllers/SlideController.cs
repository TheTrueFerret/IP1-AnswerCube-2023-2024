using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers.DTO_s;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class SlideController : Controller
{
    private readonly IManager _manager;
    private readonly ILogger<SlideController> _logger;

    public SlideController(IManager manager, ILogger<SlideController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult InfoSlide()
    {
        List<Info> infos = _manager.GetInfoSlides();
        InfoDto info = new InfoDto(infos[0]);
        return View(info);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Index()
    {
        return View();
    }
}