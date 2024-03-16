using System.ComponentModel.DataAnnotations;
using Domain;

namespace AnswerCube.BL.Domain;

public class SlideList
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public Flow? Flow { get; set; }
    public SubTheme? SubTheme { get; set; }
    public LinkedList<Slide>? Slides { get; set; }
    
}