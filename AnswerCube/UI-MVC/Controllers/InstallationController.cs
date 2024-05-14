using System.Runtime.InteropServices.JavaScript;
using AnswerCube.BL;
using AnswerCube.UI.MVC.Models.Dto;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class InstallationController : BaseController
{
    private readonly IManager _manager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwtService;
    
    public InstallationController(IManager manager, IHttpContextAccessor httpContextAccessor, JwtService jwtService)
    {
        _manager = manager;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
    }
    
    [Authorize(Roles = "Admin,DeelplatformManager,Supervisor")]
    public IActionResult ChooseInstallation()
    {
        InstallationDto installationDto = new InstallationDto();
        return View(installationDto);
    }
    
    [Authorize(Roles = "Admin,DeelplatformManager,Supervisor")]
    public IActionResult ChooseFlowForInstallation()
    {
        return View();
    }

    
    [HttpPost]
    public IActionResult SetActiveInstallation(int installationId)
    {
        string token = _jwtService.GenerateToken(installationId); // Use JwtService to generate token
        return new JsonResult(new { token });
    }
    
    [HttpPost]
    public IActionResult CreateInstallation()
    {
        return Ok();
    }
    
    
    [HttpPost]
    public IActionResult SetFlowForInstallation(int FlowId)
    {
        SlideList slideList = _manager.GetSlideListById(1);
        bool installationStarted = _manager.StartInstallation(1, slideList);
        if (installationStarted)
        {
            string token = _jwtService.GenerateToken(1); // Use JwtService to generate token
            return new JsonResult(new { token, slideList });
        }

        return null;
    }
    
}