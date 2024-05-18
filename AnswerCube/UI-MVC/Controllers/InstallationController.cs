using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Models.Dto;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using UI_MVC.Controllers;

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
        List<InstallationModel> installationModels = new List<InstallationModel>();
        List<Installation> installations = _manager.GetInstallationsByUserId(_userManager.GetUserId(User));
        foreach (var installation in installations)
        {
            installationModels.Add(
                new InstallationModel(
                    installation.Id, 
                    installation.Name, 
                    installation.Location, 
                    installation.Active, 
                    installation.CurrentSlideIndex, 
                    installation.MaxSlideIndex,
                    installation.OrganizationId,
                    installation.Organization
                ));
        }
        InstallationViewModel installationViewModel = new InstallationViewModel();
        installationViewModel.InstallationModels = installationModels;
        installationViewModel.Organizations = _manager.GetOrganizationByUserId(_userManager.GetUserId(User));
        return View(installationViewModel);
    }
    
    [Authorize(Roles = "Admin,DeelplatformManager,Supervisor")]
    public IActionResult ChooseFlowForInstallation()
    {
        List<FlowModel> flowModels = new List<FlowModel>();
        List<Flow> flows = _manager.getFlowsByUserId(_userManager.GetUserId(User));
        foreach (var flow in flows)
        {
            flowModels.Add(
                new FlowModel(
                    flow.Id, 
                    flow.Name, 
                    flow.Description, 
                    flow.CircularFlow, 
                    flow.Project, 
                    flow.ActiveInstallations, 
                    flow.SlideLists
                ));
        }
        return View(flowModels);
    }

    
    [HttpPost]
    public IActionResult SetInstallationToActive([FromBody] InstallationModel installationModel)
    {
        _manager.SetInstallationToActive(installationModel.Id);
        string token = _jwtService.GenerateToken(installationModel.Id); // Use JwtService to generate token
        string url = Url.Action("ChooseFlowForInstallation");
        return new JsonResult(new { token, url });
    }
    
    
    [HttpPost]
    public IActionResult CreateInstallation([FromBody] InstallationModel installationModel)
    {
        _manager.AddNewInstallation(installationModel.Name, installationModel.Location, installationModel.Id);
        return Ok(new { success = true, id = installationModel.Id, name = installationModel.Name, location = installationModel.Location });
    }
    
    
    [HttpPost]
    public IActionResult SetFlowForInstallation([FromForm] FlowModel flowModel)
    {
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        Installation installation = _manager.StartInstallationWithFlow(installationId, flowModel.Id);
        if (installation.Flow.CircularFlow)
        {
            return RedirectToAction("CircularFlow", "CircularFlow", new { id = installationId });
        }
        return RedirectToAction("LinearFlow", "LinearFlow", new { id = installationId });
    }
    
}