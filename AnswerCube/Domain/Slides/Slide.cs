using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnswerCube.BL.Domain.Slide;

namespace Domain;

public class Slide
{
    [Key] 
    public int Id { get; set; }
    public SlideType SlideType { get; set; }
    public string Text { get; set; } // deze text word gebruikt voor een vraag/info
    public List<string>? AnswerList { get; set; }
    
    public string? mediaUrl { get; set; } // deze url word gebruikt voor een afbeelding/video
    
    // Relational Attributes:
    [JsonIgnore]
    public ICollection<SlideConnection>? ConnectedSlideLists { get; set; }
    [JsonIgnore]
    public ICollection<Answer>? Answers { get; set; }
}