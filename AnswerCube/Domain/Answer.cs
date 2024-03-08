namespace Domain;

public class Answer
{
    private String answer { get; set; }
    private ISlide slide { get; set; }
    
    public Answer(String answer, ISlide slide)
    {
        this.answer = answer;
        this.slide = slide;
    }
}