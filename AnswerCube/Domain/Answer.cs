namespace Domain;

public class Answer
{
    private String AnswerText { get; set; }
    private int SlideId { get; set; }
    
    public Answer(String answer, int slideId)
    {
        AnswerText = answer;
        SlideId = slideId;
    }
}