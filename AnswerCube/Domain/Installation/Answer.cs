using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnswerCube.BL.Domain;

namespace Domain;

public class Answer
{
    public int Id { get; set; }
    public List<string> AnswerText { get; set; }
    public Slide? Slide { get; set; }
    [JsonIgnore]
    public Session Session { get; set; }
}