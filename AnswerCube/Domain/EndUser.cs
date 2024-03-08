namespace Domain;

public class EndUser
{
    private string name { get; set; }
    private string email { get; set; }
    private List<Idea> idea { get; set; }
    private List<Reaction> reactions { get; set; }
}