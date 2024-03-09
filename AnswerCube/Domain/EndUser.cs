namespace Domain;

public class EndUser
{
    private string Name { get; set; }
    private string Email { get; set; }
    private List<Idea> Idea { get; set; }
    private List<Reaction> Reactions { get; set; }
}