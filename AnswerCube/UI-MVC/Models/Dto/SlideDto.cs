using System.Runtime.InteropServices.JavaScript;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string? Text { get; set;} // deze text word gebruikt voor een vraag/info
    public List<String>? AnswerList { get; set; }
    
    
}