namespace AnswerCube.BL.Domain;

public class SubTheme
{
    public string Name { get; set; }
    public string Description { get; set; }

    public SubTheme(string name, string description)
    {
        Name = name;
        Description = description;
    }
}