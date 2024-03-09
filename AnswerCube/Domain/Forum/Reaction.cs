namespace Domain;

public class Reaction
{
    private EndUser EndUser { get; set; }
    private String Text { get; set; }
    private int Likes { get; set; }
    private int Dislikes { get; set; }
    
    public Reaction(EndUser endUser, String Text, int Likes, int Dislikes)
    {
        EndUser = endUser;
        this.Text = Text;
        this.Likes = Likes;
        this.Dislikes = Dislikes;
    }
}