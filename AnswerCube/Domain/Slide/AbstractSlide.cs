using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;

namespace Domain;

public abstract class AbstractSlide
{
    [Key]
    public int Id { get; set; }
    public string Text { get; set; } // deze text word gebruikt voor een vraag/info
    public SlideList? SlideList { get; set; } // Navigation property
    public ICollection<Answer>? Answers { get; set; }
}