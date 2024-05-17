using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Reaction
{
    [Key] public int Id { get; set; }
    public AnswerCubeUser? User { get; set; }
    [Required]
    [StringLength(100)]
    public String Text { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int IdeaId { get; set; }
    public Idea Idea { get; set; }
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Dislike> Dislikes { get; set; } = new List<Dislike>();
}