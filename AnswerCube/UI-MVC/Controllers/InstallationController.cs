using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Models.Dto;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class InstallationController : BaseController
{
    private readonly IManager _manager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwtService;
    private readonly UserManager<AnswerCubeUser> _userManager;
    
    public InstallationController(IManager manager, IHttpContextAccessor httpContextAccessor, JwtService jwtService, UserManager<AnswerCubeUser> userManager)
    {
        _manager = manager;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
        _userManager = userManager;
    }
    
    [Authorize(Roles = "Admin,DeelplatformManager,Supervisor")]
    public IActionResult ChooseInstallation()
    {
        InstallationModel installationModel = new InstallationModel();
        installationModel.Installations = _manager.GetInstallationsByUserId(_userManager.GetUserId(User));
        return View(installationModel);
    }
    
    [Authorize(Roles = "Admin,DeelplatformManager,Supervisor")]
    public IActionResult ChooseFlowForInstallation()
    {
        return View();
    }

    
    [HttpPost]
    public IActionResult SetInstallationToActive([FromBody] InstallationDto installationDto)
    {
        _manager.SetInstallationToActive(installationDto.Id);
        string token = _jwtService.GenerateToken(installationDto.Id); // Use JwtService to generate token
        string url = Url.Action("ChooseFlowForInstallation");
        return new JsonResult(new { token, url });
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
            return new JsonResult(new { token });
        }

        return null;
    }
    
}