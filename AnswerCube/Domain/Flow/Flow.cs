using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;

namespace Domain;

public abstract class Flow
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<SlideList>? SlideList { get; set; }
}