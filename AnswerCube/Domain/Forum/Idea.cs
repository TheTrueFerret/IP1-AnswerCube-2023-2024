namespace Domain;

public class Idea
{
    private string Title { get; set; }
    private EndUser EndUser { get; set; }
    private String Content { get; set; }
    private List<Reaction> Reactions { get; set; }
    private int Likes { get; set; }
    private int Dislikes { get; set; }
    
    public Idea(string title, EndUser endUser, String content, List<Reaction> reactions, int likes, int dislikes)
    {
        Title = title;
        EndUser = endUser;
        Content = content;
        Reactions = reactions;
        Likes = likes;
        Dislikes = dislikes;
    }
}