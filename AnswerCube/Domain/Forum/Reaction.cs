using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Reaction
{
    [Key] public int Id { get; set; }
    public AnswerCubeUser? User { get; set; }
    public String Text { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int IdeaId { get; set; }
    public Idea Idea { get; set; }
    public int Likes { get; set; } = 0;
    public int Dislikes { get; set; } = 0;
}