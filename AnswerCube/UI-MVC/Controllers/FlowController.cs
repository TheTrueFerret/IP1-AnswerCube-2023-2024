using AnswerCube.BL;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class FlowController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;
    private readonly CloudStorageService _cloudStorageService;

    public FlowController(IManager manager, ILogger<FlowController> logger,CloudStorageService cloudStorageService)
    {
        this._cloudStorageService = cloudStorageService;
        _manager = manager;
        _logger = logger;
    }

    public IActionResult FlowDetails(int flowId)
    {
        Flow flow = _manager.GetFlowWithProjectById(flowId);
        //SlideList sLideList = _manager.GetSlideListWithFlowById(flowId-1);
        var test = _manager.GetSlideListsByFlowId(flowId).ToList();
        ViewBag.SlideLists = _manager.GetSlideListsByFlowId(flowId).ToList();
        return View(flow);
    }

    public IActionResult CreateSlideView(int projectId, int slidelistId)
    {
        Project project = _manager.GetProjectById(projectId);
        ViewBag.SlideListId = slidelistId;
        ViewBag.HasCredential = _cloudStorageService.hasCredential;
        return View(project);
    }

    [HttpPost]
    public IActionResult AddSlide(string slideType, string question, string[]? options, int projectId, int slideListId,IFormFile imageFile)
    {
        SlideType type = (SlideType)Enum.Parse(typeof(SlideType), slideType);
        if (imageFile != null)
        {
            var url = _cloudStorageService.UploadFileToBucket(imageFile);
            if (_manager.CreateSlide(type, question, options, slideListId,url))
            {
                return RedirectToAction("SlideListDetails", "SlideList", new { slideListId });
            }
            return RedirectToAction("CreateSlideView");
        }
        if (_manager.CreateSlide(type, question, options, slideListId,null))
        {
            return RedirectToAction("SlideListDetails", "SlideList", new { slideListId });
        }

        //TODO: Add error message
        return RedirectToAction("CreateSlideView");
    }

    /*   public IActionResult CreateSlideListView(string title, int flowId)
       {
           bool slideList = _manager.CreateSlidelist(title, flowId);
           return View("CreateSlideListView");
       }*/

    public IActionResult CreateSlideListView(int flowId)
    {
        ViewBag.FlowId = flowId;
        return View();
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

    public IActionResult RemoveSlideFromList(int projectId, int slidelistid, int slideId)
    {
        
        if (_manager.RemoveSlideFromList(slideId, slidelistid))
        {
            return RedirectToAction("SlideListDetails", "SlideList", new { slideListId = slidelistid });
        }
        
        TempData["ErrorMessage"] = "Failed to remove slide from SlideList.";
        return RedirectToAction("SlideListDetails", "SlideList", new { slideListId = slidelistid });
        
    }

    public IActionResult AddSlideListToFlow(string title, string description, int flowId)
    {
        if (_manager.CreateSlidelist(title, description, flowId))
        {
            return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
        }
        
        TempData["ErrorMessage"] = "Failed to add SlideList to Flow.";
        return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
    }
    
    public IActionResult RemoveSlideListFromFlow(int slideListId, int flowId)
    {
        if (_manager.RemoveSlideListFromFlow(slideListId, flowId))
        {
            return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
        }
        
        TempData["ErrorMessage"] = "Failed to remove SlideList from Flow.";
        return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
    }
        
}