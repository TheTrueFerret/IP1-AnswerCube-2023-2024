using System.Diagnostics;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers;

public class FlowController : Controller
{
    public IActionResult CircularFlow()
    {
        return View();
    }
    
    public IActionResult LinearFlow()
    {
        return View();
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}