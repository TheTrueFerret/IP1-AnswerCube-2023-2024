namespace Domain;

public class Open_Question : ISlide
{
    private string question { get; set; }
    private string answer { get; set; }
    public string name { get; }
    
    public Open_Question(string name, string question, string answer)
    {
        this.name = name;
        this.question = question;
        this.answer = answer;
    }
}