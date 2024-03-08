namespace AnswerCube.BL.Domain;

public class Installation
{
    private SupInstLink SupInstLink { get; set; }
    private String Location { get; set; }
    private int Counter { get; set; }
    private bool activeFlow { get; set; }

    public Installation(SupInstLink supInstLink, string location, int counter, bool activeFlow)
    {
        SupInstLink = supInstLink;
        Location = location;
        Counter = counter;
        this.activeFlow = activeFlow;
    }
}