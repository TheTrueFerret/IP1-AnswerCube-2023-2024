using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public List<string> AnswerText { get; set; }
    public Slide? Slide { get; set; }

    
    public Answer(List<string> answerText, Slide slide)
    {
        AnswerText = answerText;
        this.Slide = slide;
    }

    public Answer(List<string> answerText)
    {
        AnswerText = answerText;
    }
}