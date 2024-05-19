using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;

namespace Domain;

public class Answer
{
    [Key]
    public int Id { get; set; }
    public List<string> AnswerText { get; set; }
    public Slide? Slide { get; set; }
    public int SessionId;
    public Session Session;
}