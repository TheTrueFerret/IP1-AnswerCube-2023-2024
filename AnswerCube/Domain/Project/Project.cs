using Domain;

namespace AnswerCube.BL.Domain.Project;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public List<Flow> Flows { get; set; } = new List<Flow>();
    public int TotalActiveInstallations { get; set; }

    public Organization Organization { get; set; }
}