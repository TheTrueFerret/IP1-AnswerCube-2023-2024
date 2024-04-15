using System.Diagnostics;
using System.Text.Json;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto;

namespace AnswerCube.UI.MVC.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class InstallationsController : Controller
{
    private readonly ILogger<InstallationsController> _logger;
    private readonly IManager _manager;
    
    
    public InstallationsController(ILogger<InstallationsController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    public void InitializeInstallation()
    {
        
    }

    

}