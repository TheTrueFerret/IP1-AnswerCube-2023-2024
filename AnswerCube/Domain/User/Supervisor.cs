using Domain;

namespace AnswerCube.BL.Domain.User;

public class Supervisor
{
    public string UserId { get; set; }
    public AnswerCubeUser User { get; set; }
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public int InstallationId { get; set; }
    public global::Domain.Installation? Installation { get; set; }
}