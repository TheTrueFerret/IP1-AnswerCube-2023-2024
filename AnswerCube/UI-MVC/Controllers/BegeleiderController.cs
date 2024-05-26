using AnswerCube.BL;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Services.SignalR;
using Domain;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Supervisor")]
public class BegeleiderController : BaseController
{
    
    private readonly FlowHub _flowHub;
    private readonly IFlowManager _flowManager;
    private readonly IInstallationManager _installationManager;
    private readonly ILogger<BegeleiderController> _logger;
    
    public BegeleiderController(IFlowManager flowManager, FlowHub flowHub, ILogger<BegeleiderController> logger)
    {
        _flowManager = flowManager;
        _flowHub = flowHub;
        _logger = logger;
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
        Flow currentFlow = _flowManager.GetFlowByInstallationId(installationId);
        _installationManager.AddNoteToInstallation(installationId, note, User.Identity.Name, DateTime.Now,currentFlow.Id);
        return RedirectToAction("FlowBeheer");
    }
    
}