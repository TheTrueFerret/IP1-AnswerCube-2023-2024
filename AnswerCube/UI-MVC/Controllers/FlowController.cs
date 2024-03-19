using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
        //LoadSlideList();
        return View(this);
    }
    
    public IActionResult LinearFlow()
    {
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = TypeSlide.Start; // Set a default condition if necessary
        }
        //LoadSlideList();
        return View(this);
    }
    
    [HttpPost]
    public IActionResult SetCurrentSlide([FromBody] SlideDto slide)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        
        CurrentCondition = slide.TypeSlide;
        return Ok();
    }
    
    
    
    
    // Method to load slide list data from the database
    private void LoadSlideList()
    {
        SlideList slideList = new SlideList();
        slideList = _manager.GetSlideListById(1);
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}