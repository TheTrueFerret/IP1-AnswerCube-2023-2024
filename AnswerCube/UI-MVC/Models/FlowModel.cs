using System.Text.Json.Serialization;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class FlowModel
{
    // Dictionary to map conditions to partial page names
    public Dictionary<SlideType, string> PartialPages { get; } = new Dictionary<SlideType, string>
    {
        { SlideType.StartSlide, "../CircularFlow/StartSlide.cshtml" },
        { SlideType.MultipleChoice, "../Slides/MultipleChoice" },
        { SlideType.SingleChoice, "../Slides/SingleChoice" },
        { SlideType.OpenQuestion, "../Slides/OpenQuestion" },
        { SlideType.Info, "../Slides/InfoSlide" },
        // Add more conditions and partial page names as needed
    };
    
    // Property to hold the current condition
    public SlideType CurrentCondition { get; set; } = SlideType.StartSlide;
    
    
    // these 2 are now in the CircularFlow or LinearFlow Javascript
    // (else we have to add cookies) maybe something for the future
    
    // public List<Slide> Slides { get; set; }
    // public int CurrentSlideIndex { get; set; }
    
}