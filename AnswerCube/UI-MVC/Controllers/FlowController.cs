using System.Diagnostics;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.UI.MVC.Controllers;
using AnswerCube.UI.MVC.Controllers.DTO_s;
using AnswerCube.UI.MVC.Models;
using AnswerCube.UI.MVC.Models.Dto;
using Domain;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers;

public class FlowController : Controller
{
    private readonly IManager _manager;
    private readonly ILogger<FlowController> _logger;

    public FlowController(IManager? manager, ILogger<FlowController>? logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult CircularFlow()
    {
        // Ensure CurrentCondition is properly set
        if (!PartialPages.ContainsKey(CurrentCondition))
        {
            CurrentCondition = "Start"; // Set a default condition if necessary
        }
        return View(this); // Pass the controller instance to the view
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

    public Dictionary<string, string> PartialPages { get; } = new Dictionary<string, string>
    {
        { "Start", "Slide/StartSlide" },
        { "MultipleChoice", "Slide/MultipleChoice" },
        { "SingleChoice", "Slide/SingleChoice" },
        { "OpenQuestion", "Slide/OpenQuestion" },
        { "InfoSlide", "Slide/InfoSlide" },
        // Add more conditions and partial page names as needed
    };

    public string CurrentCondition { get; set; } = "Start";

    public void OnGet()
    {
        // Perform any necessary initialization or data retrieval
    }

    [HttpPost]
    public IActionResult OnPostUpdateCondition(string newCondition)
    {
        CurrentCondition = newCondition; // Update the condition
        return new JsonResult(new { success = true, currentCondition = CurrentCondition });
    }

    public IActionResult InfoSlide()
    {
        InfoDto info = new InfoDto(_manager.GetInfoSlides().First());
        return View("Slide/InfoSlide", info);
    }

    // public IActionResult LinearFlow()
    // {
    //     LinearFlow lineareFlow = _manager.GetLinearFlow();
    //     //SlideList slideList = new SlideList();
    //     //if (lineareFlow.SlideList == null || lineareFlow.SlideList.Count == 0)
    //     //{
    //     //    _logger.LogInformation("HET IS HELEMAAL LEEG!!!!!!!");
    //     //    _logger.LogInformation(lineareFlow.SlideList.Count.ToString());
    //     //}
    //     //else
    //     //{
    //     //    foreach (var slide in lineareFlow.SlideList)
    //     //    {
    //     //        foreach (var nogMeerSlides in slide.Slides)
    //     //        {
    //     //            _logger.LogInformation(nogMeerSlides.GetType().ToString().ToLower());
    //     //        }
    //     //    }
    //     //    slideList = lineareFlow.SlideList.First();
    //     //}
    //
    //
    //     return View();
    // }

    [Route("api/flow/getSlideList")]
    [HttpGet]
    public IActionResult GetSlideList()
    {
        SlideList slideList = _manager.GetSlideList();
        if (slideList.Slides == null || slideList.Slides.Count == 0)
        {
            _logger.LogInformation("HET IS HELEMAAL LEEG!!!!!!!");
            return new JsonResult(new { succes = false });
        }
        else
        {
            foreach (var slide in slideList.Slides)
            {
                _logger.LogInformation(slide.GetType().ToString().ToLower());
            }
        }

        return new JsonResult(new { slideList });
    }

    [Route("api/flow/getSlideFromList/{number:int}")]
    [HttpGet]
    public IActionResult getSlideFromList(int number)
    {
        Slide slide = _manager.GetSlideFromFlow(1, number);
        if (slide == null)
        {
            _logger.LogInformation("HET IS HELEMAAL LEEG!!!!!!!");
            return new JsonResult(new { succes = false });
        }

        return new JsonResult(slide);
    }


    [Route("api/flow/getMaxNumberOfSlides")]
    [HttpGet]
    public IActionResult getMaxNumberOfSlides()
    {
        SlideList slideList = _manager.GetSlideList();
        if (slideList == null)
        {
            _logger.LogInformation("slideList is leeg!");
            return new JsonResult(new { succes = false });
        }

        return new JsonResult(new { slideList.Slides.Count });
    }

    [Route("api/flow/PostAnswer")]
    [HttpPost]
    public IActionResult PostAnswer([FromBody] AnswerModel answer)
    {
        _logger.LogInformation(answer.Answer + "-" +  answer.Id);
        int slideId = _manager.GetSlideList().Slides.ToList()[answer.Id].Id;
        List<string> answerText = answer.Answer;
        
        if (_manager.AddAnswer(answerText,slideId))
        {
            return new JsonResult(new OkResult());
        }
        else
        {
            return new JsonResult(new BadRequestResult());
        }
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}