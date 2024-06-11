using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AnswerCube.BL.Domain;
using Domain;


namespace Domain;

public class Installation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public bool Active { get; set; }
    public int CurrentSlideIndex { get; set; }
    public int? MaxSlideIndex { get; set; }
    public int? ActiveSlideListId { get; set; }
    public string? ConnectionId { get; set; }
    // Relational Attributes:
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public int? FlowId { get; set; }
    public Flow? Flow { get; set; }
    [JsonIgnore]
    public ICollection<Session>? Sessions { get; set; }
}