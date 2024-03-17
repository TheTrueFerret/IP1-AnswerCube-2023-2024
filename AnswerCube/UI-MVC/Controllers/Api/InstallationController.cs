using System.Diagnostics;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class InstallationController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public InstallationController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    
    
    
    
}