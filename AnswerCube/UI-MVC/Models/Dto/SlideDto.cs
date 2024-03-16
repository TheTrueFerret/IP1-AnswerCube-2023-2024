using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public int Id { get; set; }
    public string? Text { get; set;}
    public Boolean? IsMultipleChoice { get; set; }
    public List<String>? AnswerList { get; set; }

    public SlideDto(ListQuestion listQuestion)
    {
        Id = listQuestion.Id;
        Text = listQuestion.Text;
        IsMultipleChoice = listQuestion.IsMultipleChoice;
        AnswerList = listQuestion.AnswerList;
    }
}