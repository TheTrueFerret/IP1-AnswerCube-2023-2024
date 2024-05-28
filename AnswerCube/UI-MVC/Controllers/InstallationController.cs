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

[Authorize(Roles = "Admin,DeelplatformBeheerder,Supervisor")]
public class InstallationController : BaseController
{
    private readonly IFlowManager _flowManager;
    private readonly IOrganizationManager _organizationManager;
    private readonly IInstallationManager _installationManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtService _jwtService;
    private readonly UserManager<AnswerCubeUser> _userManager;
    private readonly ILogger<InstallationController> _logger;
    
    public InstallationController(
        IFlowManager flowManager, 
        IOrganizationManager organizationManager, 
        IInstallationManager installationManager, 
        IHttpContextAccessor httpContextAccessor, 
        JwtService jwtService, 
        UserManager<AnswerCubeUser> userManager,
        ILogger<InstallationController> logger)
    {
        _flowManager = flowManager;
        _organizationManager = organizationManager;
        _installationManager = installationManager;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
        _userManager = userManager;
        _logger = logger;
    }
    
    public IActionResult ChooseInstallation()
    {
        List<InstallationModel> installationModels = new List<InstallationModel>();
        List<Installation> installations = _installationManager.GetInstallationsByUserId(_userManager.GetUserId(User));
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
        installationViewModel.Organizations = _organizationManager.GetOrganizationByUserId(_userManager.GetUserId(User));
        return View(installationViewModel);
    }
    
    public IActionResult ChooseFlowForInstallation()
    {
        List<FlowModel> flowModels = new List<FlowModel>();
        List<Flow> flows = _flowManager.GetFlowsByUserId(_userManager.GetUserId(User));
        foreach (var flow in flows)
        {
            flowModels.Add(
                new FlowModel()
                {
                    Id = flow.Id, 
                    Name = flow.Name, 
                    Description = flow.Description, 
                    CircularFlow = flow.CircularFlow, 
                    Project = flow.Project, 
                    ActiveInstallations = flow.ActiveInstallations
                });
        }
        return View(flowModels);
    }

    [HttpPost]
    [Route("Installation/SetInstallationUrl/{url}")]
    public IActionResult SetInstallationUrl(string url)
    {
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        _installationManager.SetInstallationUrl(installationId, url);
        return Ok();
    }
    
    
    [HttpPost]
    public IActionResult SetInstallationToActive([FromBody] InstallationModel installationModel)
    {
        _installationManager.SetInstallationToActive(installationModel.Id);
        string token = _jwtService.GenerateToken(installationModel.Id); // Use JwtService to generate token
        string url = Url.Action("ChooseFlowForInstallation");
        return new JsonResult(new { token, url });
    }
    
    
    [HttpPost]
    public IActionResult CreateInstallation([FromBody] InstallationModel installationModel)
    {
        _installationManager.AddNewInstallation(installationModel.Name, installationModel.Location, installationModel.Id);
        return Ok(new { success = true, id = installationModel.Id, name = installationModel.Name, location = installationModel.Location });
    }
    
    
    [HttpPost]
    public IActionResult SetFlowForInstallation([FromForm] FlowModel flowModel)
    {
        string token = Request.Cookies["jwtToken"];
        int installationId = _jwtService.GetInstallationIdFromToken(token);
        
        Installation installation = _installationManager.StartInstallationWithFlow(installationId, flowModel.Id);
        if (installation.Flow.CircularFlow)
        {
            return RedirectToAction("CircularFlow", "CircularFlow", new { id = installationId });
        }
        return RedirectToAction("LinearFlow", "LinearFlow", new { id = installationId });
    }
}