using System.ComponentModel.DataAnnotations;
using Domain;


namespace AnswerCube.BL.Domain;

public class Installation
{
    [Key]
    public int Id { get; set; }
    public string Location { get; set; }
    public bool Active { get; set; }
    public int CurrentSlideIndex { get; set; }
    public int MaxSlideIndex { get; set; }
    public List<global::Domain.Slide>? Slides { get; set; }
    public int ActiveSlideListId { get; set; }
    public Flow? Flow { get; set; }
}