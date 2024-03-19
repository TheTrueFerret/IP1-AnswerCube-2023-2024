using AnswerCube.BL.Domain.Slide;
using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public int Id { get; set; }
    public TypeSlide TypeSlide { get; set; }
    public string? Text { get; set;}
    public Boolean? IsMultipleChoice { get; set; }
    public List<String>? AnswerList { get; set; }
    
    public SlideDto(AbstractSlide slide)
    {
        Id = slide.Id;
        TypeSlide = slide.TypeSlide;
        Text = slide.Text;
    }
    
    public SlideDto(ListQuestion listQuestion)
    {
        Id = listQuestion.Id;
        TypeSlide = listQuestion.TypeSlide;
        Text = listQuestion.Text;
        AnswerList = listQuestion.AnswerList;
    }
    
    
    public SlideDto(OpenQuestion OpenQuestion)
    {
        Id = OpenQuestion.Id;
        TypeSlide = OpenQuestion.TypeSlide;
        Text = OpenQuestion.Text;
    }
    
}