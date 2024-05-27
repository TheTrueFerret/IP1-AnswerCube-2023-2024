using AnswerCube.BL;
using Microsoft.AspNetCore.SignalR;

namespace AnswerCube.UI.MVC.Services.SignalR;

public class FlowHub : Hub
{
    private readonly IFlowManager _flowManager;
    private readonly IInstallationManager _installationManager;
    private readonly ILogger<FlowHub> _logger;
    
    public FlowHub(IFlowManager flowManager, IInstallationManager installationManager, ILogger<FlowHub> logger)
    {
        _flowManager = flowManager;
        _installationManager = installationManager;
        _logger = logger;
    }
    
    public async Task StopFlow(string installationId)
    {
        if (Clients == null)
        {
            _logger.LogError("Clients is null");
        }
        else if (Clients.All == null)
        {
            _logger.LogError("Clients.All is null");
        }
        else
        {
            await Clients.All.SendAsync("FlowStopped", installationId);
        }
    }
    
    public async Task StartFlow(string installationId)
    {
        await Clients.All.SendAsync("FlowStarted", installationId);
    }
    
}