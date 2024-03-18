using AnswerCube.BL.Domain.Slide;
using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public int Id { get; set; }
    public string? Text { get; set;}
    public Boolean? IsMultipleChoice { get; set; }
    public List<String>? AnswerList { get; set; }

    public SlideDto(ListQuestion slide)
    {
        Id = slide.Id;
        Text = slide.Text;
        
    }
    
    public SlideDto(OpenQuestion slide)
    {
        Id = slide.Id;
        Text = slide.Text;
    }
    
    public SlideDto(Info slide)
    {
        Id = slide.Id;
        Text = slide.Text;
    }
    
    public SlideDto(RequestingInfo slide)
    {
        Id = slide.Id;
        Text = slide.Text;
    }
    
    public SlideDto(AbstractSlide slide)
    {
        Id = slide.Id;
        Text = slide.Text;
    }
}