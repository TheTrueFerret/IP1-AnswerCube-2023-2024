using System.ComponentModel.DataAnnotations;
using Domain;

namespace AnswerCube.BL.Domain;

public class SubTheme
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<SlideList>? SlideList { get; set; }

    public SubTheme(string name)
    {
        Name = name;
    }
    
    public SubTheme(string name, string description)
    {
        Name = name;
        Description = description;
    }
}