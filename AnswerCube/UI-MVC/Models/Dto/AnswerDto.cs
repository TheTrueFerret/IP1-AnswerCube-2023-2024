using Domain;

namespace AnswerCube.UI.MVC.Models.Dto;

public class AnswerDto
{
    public Answer Answer { get; set; }
    
    public AnswerDto(Answer answer)
    {
        Answer = answer;
    }
}