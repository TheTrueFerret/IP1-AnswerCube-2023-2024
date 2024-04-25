using AnswerCube.BL.Domain;

namespace Domain;

public class Results
{
    private ICollection<Session> Sessions { get; set; }
    
    public Results(List<Session> sessions)
    {
        Sessions = sessions;
    }
}