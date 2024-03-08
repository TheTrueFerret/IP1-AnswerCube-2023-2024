namespace Domain;

public class Info : ISlide
{
    public string name { get; }
    private string info { get; set; }
    
    public Info(string name, string info)
    {
        this.name = name;
        this.info = info;
    }
}