namespace AnswerCube.BL.Domain.User;

public class DeelplatformbeheerderEmail
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string? DeelplatformNaam { get; set; }
    public bool IsDeelplatformBeheerder { get; set; }
}