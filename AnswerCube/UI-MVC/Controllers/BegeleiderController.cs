using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services.SignalR;
using Domain;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Supervisor")]
public class BegeleiderController : BaseController
{
    
    private readonly FlowHub _flowHub;
    private readonly IFlowManager _flowManager;
    private readonly IInstallationManager _installationManager;
    private readonly ILogger<BegeleiderController> _logger;
    private readonly UserManager<AnswerCubeUser> _userManager;
    
    public BegeleiderController(IFlowManager flowManager, FlowHub flowHub, ILogger<BegeleiderController> logger, UserManager<AnswerCubeUser> userManager, IInstallationManager installationManager)
    {
        _flowManager = flowManager;
        _flowHub = flowHub;
        _logger = logger;
        _userManager = userManager;
        _installationManager = installationManager;
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
    
    public async Task<IActionResult> StartFlow(string installationId)
    {
        await _flowHub.StartFlow(installationId);
        return RedirectToAction("FlowBeheer");
    }
    
    public async Task<IActionResult> StopFlow(string installationId)
    {
        await _flowHub.StopFlow(installationId);
        return RedirectToAction("FlowBeheer");
    }
    
    public IActionResult AddNote(int installationId, string note)
    {
        //TODO: get flowId from installation and installationId from website.
        //Flow currentFlow = _flowManager.GetFlowByInstallationId(installationId);
        AnswerCubeUser user = _userManager.GetUserAsync(User).Result;
        _installationManager.AddNoteToInstallation(1, note, user.Email,1);
        return RedirectToAction("FlowBeheer");
    }
    
}