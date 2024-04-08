using AnswerCube.BL.Domain.Slide;
using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string? Text { get; set;}
    public Boolean? IsMultipleChoice { get; set; }
    public List<String>? AnswerList { get; set; }
    
    
    public SlideDto(Slide slide)
    {
        Id = slide.Id;
        SlideType = slide.SlideType;
        Text = slide.Text;
    }
    
    
}