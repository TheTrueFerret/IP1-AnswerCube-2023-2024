using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;

namespace Domain;

public class Flow
{
    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool CircularFlow { get; set; }

    public Project? Project { get; set; }

    public ICollection<Installation>? ActiveInstallations { get; set; }
    public ICollection<SlideList>? SlideList { get; set; }
}