namespace Domain;

public class Idea
{
    private string title { get; set; }
    private EndUser endUser { get; set; }
    private String content { get; set; }
    private List<Reaction> reactions { get; set; }
    private int Likes { get; set; }
    private int Dislikes { get; set; }
}