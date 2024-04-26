using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnswerCube.BL.Domain;
using Domain;

namespace Domain;

public class SlideList
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    
    public int FlowId { get; set; }
    
    // Relational Attributes:
    public Flow? Flow { get; set; }
    public SubTheme? SubTheme { get; set; }
    [JsonIgnore]
    public List<SlideConnection>? ConnectedSlides { get; set; }
    
}