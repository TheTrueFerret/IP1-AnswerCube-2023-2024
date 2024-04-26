using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;

namespace Domain;

public class Flow
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public bool CircularFlow { get; set; } 
    public ICollection<Installation>?  ActiveInstallations { get; set; }
    public ICollection<SlideList>? SlideList { get; set; }
}