namespace AnswerCube.BL.Domain.User;

public class UserClaim
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }

    // Navigation property
    public AnswerCubeUser User { get; set; }
}