using Domain;

namespace AnswerCube.BL.Domain.User;

public class UserOrganization
{
    public string UserId { get; set; }
    public AnswerCubeUser User { get; set; }

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
}