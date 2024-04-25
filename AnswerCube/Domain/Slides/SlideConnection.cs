using System.ComponentModel.DataAnnotations;
using Domain;


public class SlideConnection
{
    [Key]
    public int Id { get; set; }
    public SlideList SlideList { get; set; }
    public Slide Slide { get; set; }
    public int SlideOrder { get; set; }
}