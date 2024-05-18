using Domain;

namespace AnswerCube.BL.Domain;

public class Session
{
    public int Id { get; set; }
    public int CubeId { get; set; }
    public Installation Installation { get; set; }
    public ICollection<Answer>? Answers { get; set; } = new List<Answer>();
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }

    public Session()
    {
        StartTime = DateTime.Now;
    }
}