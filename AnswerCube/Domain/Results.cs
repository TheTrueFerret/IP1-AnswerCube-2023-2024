using AnswerCube.BL.Domain;

namespace Domain;

public class Results
{
    private List<Session> Sessions { get; set; }
    
    public Results(List<Session> sessions)
    {
        Sessions = sessions;
    }
}