using AnswerCube.BL.Domain.Project;

namespace Domain;

public class Forum
{
    private Project Project { get; set; }
    private List<Idea> Ideas { get; set; }
    
    public Forum(Project project, List<Idea> ideas)
    {
        Project = project;
        Ideas = ideas;
    }
}