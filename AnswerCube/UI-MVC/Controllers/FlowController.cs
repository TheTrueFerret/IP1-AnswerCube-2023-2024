using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers;

public class FlowController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public FlowController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    public IActionResult LinearFlow()
    {
        return View();
    }
    
    public IActionResult CircularFlow()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}