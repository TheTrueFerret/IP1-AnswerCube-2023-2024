using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Models;
using Domain;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;

namespace UI_MVC.Controllers;

public class FlowController : Controller
{
    private readonly IManager _manager;

    public FlowController(IManager manager)
    {
        _manager = manager;
    }
    
    // Dictionary to map conditions to partial page names
    public Dictionary<string, string> PartialPages { get; } = new Dictionary<string, string>
    {
        { "Start", "Slide/StartSlide" },
        { "MultipleChoice", "Slide/MultipleChoice" },
        { "SingleChoice", "Slide/SingleChoice" },
        { "OpenQuestion", "Slide/OpenQuestion" },
        { "InfoSlide", "Slide/InfoSlide" },
        // Add more conditions and partial page names as needed
    };
    // Property to hold the current condition
    public string CurrentCondition { get; set; } = "Start";
    
    
    public IActionResult CircularFlow()
    {
        // Ensure CurrentCondition is properly set
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = "Start"; // Set a default condition if necessary
        }

        List<CircularFlow> cFlow = _manager.GetCirculaireFlow();
        
        return View(cFlow); // Pass the controller instance to the view
    }
    
    public IActionResult LinearFlow()
    {
        // Ensure CurrentCondition is properly set
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = "Start"; // Set a default condition if necessary
        }
        return View(this); // Pass the controller instance to the view
    }
    
    [HttpGet]
    public void SetCurrentCondition()
    {
        // Perform any necessary initialization or data retrieval
        // TODO getting the data
    }
    
    [HttpPost]
    public IActionResult OnPostUpdateCondition(string newCondition)
    {
        CurrentCondition = newCondition; // Update the condition
        return new JsonResult(new { success = true, currentCondition = CurrentCondition });
    }
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}