using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class FlowController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IManager _manager;
    
    // Dictionary to map conditions to partial page names
    public Dictionary<TypeSlide, string> PartialPages { get; } = new Dictionary<TypeSlide, string>
    {
        { TypeSlide.Start, "Slide/StartSlide" },
        { TypeSlide.MultipleChoice, "Slide/MultipleChoice" },
        { TypeSlide.SingleChoice, "Slide/SingleChoice" },
        { TypeSlide.OpenQuestion, "Slide/OpenQuestion" },
        { TypeSlide.Info, "Slide/InfoSlide" },
        // Add more conditions and partial page names as needed
    };

    // Property to hold the current condition
    public TypeSlide CurrentCondition { get; set; } = TypeSlide.Start;
    
    // List of slides in order
    private List<SlideList> slideList;
    private int currentSlideIndex = 0;
    
    public FlowController(ILogger<HomeController> logger, IManager manager)
    {
        _logger = logger;
        _manager = manager;
    }
    
    public IActionResult CircularFlow()
    {
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = TypeSlide.Start; // Set a default condition if necessary
        }
        return View(this);
    }
    
    public IActionResult LinearFlow()
    {
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = TypeSlide.Start; // Set a default condition if necessary
        }
        return View(this);
    }
    
    private string GetPartialViewName(TypeSlide condition)
    {
        return PartialPages.ContainsKey(condition) ? PartialPages[condition] : "Slide/StartSlide"; // Provide a default partial view if necessary
    }
    
    [HttpPost]
    public IActionResult SetCurrentSlide([FromBody] SlideDto slideDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        if (slideDto == null)
            return NotFound("Game or GameStore not found");
        
        // Check if the received slide type exists in PartialPages
        if (PartialPages.ContainsKey(slideDto.TypeSlide))
        {
            CurrentCondition = slideDto.TypeSlide;
            string partialViewName = GetPartialViewName(CurrentCondition);
            return PartialView(partialViewName); // Return the partial view
        }
        else
        {
            return NotFound("Slide not found");
        }
    }
    
    
    // Helper method to render partial view to string
    public string RenderPartialViewToString(Controller controller, string viewName, object model = null)
    {
        controller.ViewData.Model = model;
        using (var sw = new StringWriter())
        {
            var engine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            var viewResult = engine.FindView(controller.ControllerContext, viewName, false);
            var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
            viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
    
    
    // method to get the active slide id
    [HttpGet]
    public int getActiveSlideId()
    {
        return 1;
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}