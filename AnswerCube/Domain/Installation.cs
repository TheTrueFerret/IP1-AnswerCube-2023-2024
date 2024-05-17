using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;
using Domain;


namespace Domain;

public class Installation
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public bool Active { get; set; }
    public int CurrentSlideIndex { get; set; }
    public int MaxSlideIndex { get; set; }
    public int ActiveSlideListId { get; set; }
    
    public Organization Organization { get; set; }
    // Relational Attributes:
    public int? FlowId { get; set; }
    public Flow? Flow { get; set; }
}