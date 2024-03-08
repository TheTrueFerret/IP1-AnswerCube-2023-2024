namespace Domain;

public class Reaction
{
    private EndUser endUser { get; set; }
    private String reaction { get; set; }
    private int Likes { get; set; }
    private int Dislikes { get; set; }
    
    public Reaction(EndUser endUser, String reaction, int Likes, int Dislikes)
    {
        this.endUser = endUser;
        this.reaction = reaction;
        this.Likes = Likes;
        this.Dislikes = Dislikes;
    }
}