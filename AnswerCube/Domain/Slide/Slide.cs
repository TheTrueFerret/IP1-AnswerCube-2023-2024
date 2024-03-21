using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;

namespace Domain;

public class Slide
{
    [Key] public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string? Info { get; set; }
    public string Text { get; set; } // deze text word gebruikt voor een vraag/info
    public SlideList? SlideList { get; set; } // Navigation property
    public List<String>? AnswerList { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}