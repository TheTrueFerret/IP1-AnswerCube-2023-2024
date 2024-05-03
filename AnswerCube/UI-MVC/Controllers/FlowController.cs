using AnswerCube.BL;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class FlowController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;

    public FlowController(IManager manager, ILogger<FlowController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult FlowDetails(int flowId)
    {
        Flow flow = _manager.GetFlowWithProjectById(flowId);
        return View(flow);
    }

    public IActionResult CreateSlideView(int projectId)
    {
        Project project = _manager.GetProjectById(projectId);
        return View(project);
    }

    [HttpPost]
    public IActionResult AddSlide(string slideType, string question, string[]? options, int projectId)
    {
        SlideType type = (SlideType)Enum.Parse(typeof(SlideType), slideType);
        if (_manager.CreateSlide(type, question, options))
        {
            return RedirectToAction("Flows", "Project", new { projectId });
        }

        //TODO: Add error message
        return RedirectToAction("CreateSlideView");
    }

    [HttpPost]
    public IActionResult AddFlow(string name, string desc, string flowType, int projectId)
    {
        //TODO: Add flow to project with ID that we get trough the website
        bool circularFlow = flowType is "circular" or not "linear";

        if (_manager.CreateFlow(name, desc, circularFlow, projectId))
        {
            return RedirectToAction("Flows", "Project", new { projectId });
        }

        return RedirectToAction("NewFlowView", "Project", new { projectId });
    }


    public IActionResult ShowSlides()
    {
        var slides = _manager.GetAllSlides();
        return View(slides);
    }

    public IActionResult EditFlowView(int flowid)
    {
        Flow flow = _manager.GetFlowWithProjectById(flowid);
        return View(flow);
    }

    [HttpPost]
    public IActionResult EditFlow(Flow model)
    {
        if (ModelState.IsValid)
        {
            _manager.UpdateFlow(model);
            return RedirectToAction("FlowDetails", new { flowId = model.Id });
        }

        return RedirectToAction("EditFlowView", new { flowid = model.Id });
    }
}