namespace Domain;

public class Results
{
    private List<Session> sessions { get; set; }

    public Results(List<Session> sessions)
    {
        this.sessions = sessions;
    }
}