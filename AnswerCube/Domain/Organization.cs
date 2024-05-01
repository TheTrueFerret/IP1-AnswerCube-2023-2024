using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Organization
{
    private string Name { get; set; }
    private string Email { get; set; }
    private List<Project> Projects { get; set; } = new List<Project>();
    public List<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    
    public Organization(string name, string email)
    {
        Name = name;
        Email = email;
    }
}