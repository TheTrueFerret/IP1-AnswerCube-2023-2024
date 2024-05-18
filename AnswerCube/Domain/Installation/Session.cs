using Domain;

namespace AnswerCube.BL.Domain;

public class Session
{
    public int Id { get; set; }
    public int CubeId { get; set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public Installation Installation { get; set; }
    public ICollection<Answer>? Answers { get; set; } = new List<Answer>();

    public Session()
    {
        StartTime = DateTime.Now.ToUniversalTime();
    }
}