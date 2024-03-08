namespace Domain;

public class Idea
{
    private string title { get; set; }
    private EndUser endUser { get; set; }
    private String content { get; set; }
    private List<Reaction> reactions { get; set; }
    private int Likes { get; set; }
    private int Dislikes { get; set; }
    
    public Idea(string title, EndUser endUser, String content, List<Reaction> reactions, int Likes, int Dislikes)
    {
        this.title = title;
        this.endUser = endUser;
        this.content = content;
        this.reactions = reactions;
        this.Likes = Likes;
        this.Dislikes = Dislikes;
    }
}