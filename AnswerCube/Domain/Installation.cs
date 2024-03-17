using System.ComponentModel.DataAnnotations;
using Domain;

namespace AnswerCube.BL.Domain;

public class Installation
{
    [Key]
    public int Id { get; set; }
    public string Location { get; set; }
    public int Counter { get; set; }
    public Flow? Flow { get; set; }
}