namespace Domain;

public class Session
{
    private Installation installation { get; set; }
    private List<Answer> answers { get; set; }
    private DateTime startTime { get; set; }
    private DateTime endTime { get; set; }
    
    public Session(Installation installation, List<Answer> answers, DateTime startTime, DateTime endTime)
    {
        this.installation = installation;
        this.answers = answers;
        this.startTime = startTime;
        this.endTime = endTime;
    }
}