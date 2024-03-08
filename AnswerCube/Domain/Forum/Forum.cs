namespace Domain;

public class Forum
{
    private Project project { get; set; }
    private List<Idea> ideas { get; set; }
    
    public Forum(Project project, List<Idea> ideas)
    {
        this.project = project;
        this.ideas = ideas;
    }
}