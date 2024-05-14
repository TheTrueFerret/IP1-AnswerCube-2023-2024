using AnswerCube.BL.Domain.Project;

namespace Domain;

public class Forum
{
    public int Id { get; set; }
    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
    public List<Idea> Ideas { get; set; }
    
    public Forum()
    {
    }

    public Forum(Organization organization, List<Idea> ideas)
    {
        Organization = organization;
        Ideas = ideas;
    }
}