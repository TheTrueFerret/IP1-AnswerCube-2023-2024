using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services.SignalR;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Supervisor,Admin")]
public class BegeleiderController : BaseController
{
    private readonly IHubContext<FlowHub> _flowHub;
    private readonly IFlowManager _flowManager;
    private readonly IOrganizationManager _organizationManager;
    private readonly IInstallationManager _installationManager;
    private readonly ILogger<BegeleiderController> _logger;
    private readonly UserManager<AnswerCubeUser> _userManager;
    private readonly UnitOfWork _uow;
    
    public BegeleiderController(IOrganizationManager organizationManager, IHubContext<FlowHub> flowHub, ILogger<BegeleiderController> logger, UserManager<AnswerCubeUser> userManager, IInstallationManager installationManager, IFlowManager flowManager, UnitOfWork uow)
    {
        _organizationManager = organizationManager;
        _flowHub = flowHub;
        _logger = logger;
        _userManager = userManager;
        _installationManager = installationManager;
        _flowManager = flowManager;
        _uow = uow;
    }
    
    public IActionResult SelectActiveInstallation()
    {
        List<Organization> organizations = _organizationManager.GetOrganizationByUserId(_userManager.GetUserId(User));
        List<Installation> installations = _installationManager.GetActiveInstallationsFromOrganizations(organizations);
        return View(installations);
    }
    
    public IActionResult FlowBeheer(int installationId)
    {
        NoteModel model = new NoteModel
        {
            InstallationId = installationId,
            Note = ""
        };
        return View(model);
    }
    

    [HttpPost]
    public async Task<IActionResult> StartFlow(int installationId)
    {
        string connectionId = _installationManager.GetConnectionIdByInstallationId(installationId);
        await _flowHub.Clients.Client(connectionId).SendAsync("StartFlow");
        return RedirectToAction("FlowBeheer", new { installationId = installationId});
    }
    

    [HttpPost]
    public async Task<IActionResult> StopFlow(int installationId)
    {
        string connectionId = _installationManager.GetConnectionIdByInstallationId(installationId);
        await _flowHub.Clients.Client(connectionId).SendAsync("StopFlow");
        return RedirectToAction("FlowBeheer", new { installationId = installationId});
    }
    
    public IActionResult AddNote(string note, int installationId)
    {
        Flow currentFlow = _flowManager.GetFlowByInstallationId(installationId);
        AnswerCubeUser user = _userManager.GetUserAsync(User).Result;
        _uow.BeginTransaction();
        _installationManager.AddNoteToInstallation(installationId, note, user.Email,currentFlow.Id);
        _uow.Commit();

        return RedirectToAction("FlowBeheer", new { installationId = installationId});
    }
    
}