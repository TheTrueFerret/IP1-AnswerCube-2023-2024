namespace AnswerCube.BL.Domain.Project;

public class Project
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public int TotalFlows { get; set; }
    public int TotalActiveInstallations { get; set; }
    public List<Project> Projects { get; set; }
}
