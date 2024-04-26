using System.ComponentModel.DataAnnotations;
using Domain;


public class SlideConnection
{
    public int Id { get; set; }
    public int SlideOrder { get; set; }
    
    // Relational Attributes:
    public int SlideListId { get; set; }
    public int SlideId { get; set; }
    public SlideList SlideList { get; set; } = null!;
    public Slide Slide { get; set; } = null!;
}