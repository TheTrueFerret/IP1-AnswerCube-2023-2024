namespace AnswerCube.BL.Domain;

public class SupInstLink
{
    private Installation _installation { get; set; }
    private String Notes { get; set; }

    public SupInstLink(Installation installation, string notes)
    {
        _installation = installation;
        Notes = notes;
    }
}