using System.Text.Json.Serialization;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class FlowModel
{
    // Property to hold the current condition
    public SlideType CurrentCondition { get; set; } = SlideType.StartSlide;
    
    // these 2 are now in the CircularFlow or LinearFlow Javascript
    // (else we have to add cookies) maybe something for the future
    
    public List<Slide> Slides { get; set; }
    
    public SlideList ActiveSlideList { get; set; }
    
    public int CurrentSlideIndex { get; set; }
    
    public int MaxSlide { get; set; }
}