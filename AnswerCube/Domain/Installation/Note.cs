namespace AnswerCube.BL.Domain.Installation;

public class Note
{
    public int Id { get; set; }
    public string NoteText { get; set; }
    public string? IdentityName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FlowId { get; set; }
    public int InstallationId { get; set; }
}