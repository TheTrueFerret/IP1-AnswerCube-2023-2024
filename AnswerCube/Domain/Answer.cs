using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public string AnswerText { get; set; }
    public Slide Slide { get; set; }
    
}