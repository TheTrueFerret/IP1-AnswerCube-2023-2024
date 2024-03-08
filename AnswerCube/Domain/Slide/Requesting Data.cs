namespace Domain;

public class Requesting_Data : ISlide
{
    private String question { get; set; }
    private String email { get; set; }
    public string name { get; }
    
    public Requesting_Data(string name, String question, String email)
    {
        this.name = name;
        this.question = question;
        this.email = email;
    }
}