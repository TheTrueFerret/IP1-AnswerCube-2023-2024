using Domain;

namespace AnswerCube.BL.Domain;

public class Session
{
    private readonly Installation _installation;
    private readonly List<Answer> _answers;
    private DateTime StartTime { get; set; }
    private DateTime EndTime { get; set; }
    public Session(Installation installation, List<Answer> answers, DateTime startTime, DateTime endTime)
    {
        _installation = installation;
        _answers = answers;
        StartTime = startTime;
        EndTime = endTime;
    }
}