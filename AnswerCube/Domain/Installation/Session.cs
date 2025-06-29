using Domain;

namespace AnswerCube.BL.Domain;

public class Session
{
    public int Id { get; set; }
    public int CubeId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public global::Domain.Installation Installation { get; set; }
    public ICollection<Answer>? Answers { get; set; }

    public Session()
    {
        StartTime = DateTime.Now.ToUniversalTime();
    }
}