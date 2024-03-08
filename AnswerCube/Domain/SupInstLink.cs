namespace AnswerCube.BL.Domain;

public class SupInstLink
{
    private Installation _installation { get; set; }
    private String notes { get; set; }

    public SupInstLink(Installation installation, string notes)
    {
        _installation = installation;
        this.notes = notes;
    }
}