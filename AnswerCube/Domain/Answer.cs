using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Answer
{
    [Key] public int Id { get; set; }
    public List<string> AnswerText { get; set; }
    public Slide? slide { get; set; }

    public Answer(List<string> answerText, Slide slide)
    {
        AnswerText = answerText;
        this.slide = slide;
    }

    public Answer(List<string> answerText)
    {
        AnswerText = answerText;
    }
}