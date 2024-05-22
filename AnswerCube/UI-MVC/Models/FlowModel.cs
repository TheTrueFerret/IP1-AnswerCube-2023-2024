using AnswerCube.BL.Domain.Project;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class FlowModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool CircularFlow { get; set; }
    public Project? Project { get; set; }
    public ICollection<Installation>? ActiveInstallations { get; set; }
    public ICollection<SlideList>? SlideLists { get; set; }
}