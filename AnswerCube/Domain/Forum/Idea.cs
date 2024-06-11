using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Idea
{
    [Key] public int Id { get; set; }
    [Required]public string Title { get; set; }

    public AnswerCubeUser? User { get; set; }

    [Required] public String Content { get; set; }
    public List<Reaction> Reactions { get; set; } = new List<Reaction>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<Dislike> Dislikes { get; set; } = new List<Dislike>();

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int ForumId { get; set; }

    public Forum Forum { get; set; }

    public Idea()
    {
    }

    public Idea(string title, string content, Forum forum)
    {
        Title = title;
        Content = content;
        Forum = forum;
    }
}