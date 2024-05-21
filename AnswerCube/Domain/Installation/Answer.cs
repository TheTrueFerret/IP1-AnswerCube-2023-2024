using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnswerCube.BL.Domain;

namespace Domain;

public class Answer
{
    public int Id { get; set; }
    public List<string> AnswerText { get; set; }
    public Slide? Slide { get; set; }
    public Session Session { get; set; }
}