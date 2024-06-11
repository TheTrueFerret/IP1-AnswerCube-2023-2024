using AnswerCube.BL;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class FlowController : BaseController
{
    private readonly IOrganizationManager _organizationManager;
    private readonly IFlowManager _flowManager;
    private readonly ILogger<FlowController> _logger;
    private readonly CloudStorageService _cloudStorageService;
    private readonly UnitOfWork _uow;
    
    public FlowController(IOrganizationManager organizationManager, IFlowManager flowManager, ILogger<FlowController> logger, CloudStorageService cloudStorageService, UnitOfWork uow)
    {
        _organizationManager = organizationManager;
        _flowManager = flowManager;
        _logger = logger;
        _cloudStorageService = cloudStorageService;
        _uow = uow;
    }

    public IActionResult FlowDetails(int flowId)
    {
        Flow flow = _flowManager.GetFlowWithProjectById(flowId);
        ViewBag.SlideLists = _flowManager.GetSlideListsByFlowId(flowId).ToList();
        ViewBag.Flow = _flowManager.GetFlowWithProjectById(flowId);
        ViewBag.Notes = _flowManager.GetNotesByFlowId(flowId).ToList();
        return View(flow);
    }

    public IActionResult CreateSlideView(int projectId, int slidelistId)
    {
        Project project = _organizationManager.GetProjectById(projectId);
        ViewBag.SlideListId = slidelistId;
        ViewBag.HasCredential = _cloudStorageService.hasCredential;
        return View(project);
    }

    [HttpPost]
    public IActionResult AddSlide(string slideType, string question, List<string>? options, int projectId, int slideListId,IFormFile imageFile)
    {
        SlideType type = (SlideType)Enum.Parse(typeof(SlideType), slideType);
        if (imageFile != null)
        {
            _uow.BeginTransaction();
            var url = _cloudStorageService.UploadFileToBucket(imageFile);
            _uow.Commit();
            _uow.BeginTransaction();
            if (_flowManager.CreateSlide(type, question, options, slideListId, url))
            {
                _uow.Commit();
                return RedirectToAction("SlideListDetails", "SlideList", new { slideListId });
            }
            return RedirectToAction("CreateSlideView");
        }
        _uow.BeginTransaction();
        if (_flowManager.CreateSlide(type, question, options, slideListId,null))
        {
            _uow.Commit();
            return RedirectToAction("SlideListDetails", "SlideList", new { slideListId });
        }

        TempData["ErrorMessage"] = "Failed to add slide.";
        return RedirectToAction("CreateSlideView");
        
    }
    

    public IActionResult CreateSlideListView(int flowId)
    {
        ViewBag.FlowId = flowId;
        return View();
    }
    

    [HttpPost]
    public IActionResult AddFlow(string name, string desc, string flowType, int projectId)
    {
        bool circularFlow = flowType is "circular" or not "linear";
        
        _uow.BeginTransaction();
        if (_flowManager.CreateFlow(name, desc, circularFlow, projectId))
        {
            _uow.Commit();
            return RedirectToAction("Flows", "Project", new { projectId });
        }

        return RedirectToAction("NewFlowView", "Project", new { projectId });
    }

    public IActionResult ShowSlides()
    {
        var slides = _flowManager.GetAllSlides();
        return View(slides);
    }
    
    
    public IActionResult EditFlowView(int flowid)
    {
        Flow flow = _flowManager.GetFlowWithProjectById(flowid);
        return View(flow);
    }

    [HttpPost]
    public IActionResult EditFlow(Flow model)
    {
        if (ModelState.IsValid)
        {
            _uow.BeginTransaction();
            _flowManager.UpdateFlow(model);
            _uow.Commit();
            return RedirectToAction("FlowDetails", new { flowId = model.Id });
        }

        return RedirectToAction("EditFlowView", new { flowid = model.Id });
    }

    public IActionResult DeleteFlow(int flowId, int projectId)
    {
        _uow.BeginTransaction();
        if (_flowManager.RemoveFlowFromProject(flowId))
        {
            _uow.Commit();
            Flow flow = _flowManager.GetFlowById(flowId);
            return RedirectToAction("Flows", "Project", new { projectId = projectId });
        }
        else
        {
            return View("Error");
        }
    }
    
    public IActionResult RemoveSlideFromList(int projectId, int slidelistid, int slideId)
    {
        _uow.BeginTransaction();
        if (_flowManager.RemoveSlideFromSlideList(slideId, slidelistid))
        {
            _uow.Commit();
            return RedirectToAction("SlideListDetails", "SlideList", new { slideListId = slidelistid });
        }
        
        TempData["ErrorMessage"] = "Failed to remove slide from SlideList.";
        return RedirectToAction("SlideListDetails", "SlideList", new { slideListId = slidelistid });
        
    }

    public IActionResult AddSlideListToFlow(string title, string description, int flowId)
    {
        _uow.BeginTransaction();
        if (_flowManager.CreateSlidelist(title, description, flowId))
        {
            _uow.Commit();
            return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
        }
        
        TempData["ErrorMessage"] = "Failed to add SlideList to Flow.";
        return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
    }
    
    public IActionResult RemoveSlideListFromFlow(int slideListId, int flowId)
    {
        _uow.BeginTransaction();
        if (_flowManager.RemoveSlideListFromFlow(slideListId, flowId))
        {
            _uow.Commit();
            return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
        }
        
        TempData["ErrorMessage"] = "Failed to remove SlideList from Flow.";
        return RedirectToAction("FlowDetails", "Flow", new { flowId = flowId });
    }
}