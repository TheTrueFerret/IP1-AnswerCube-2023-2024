using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
    public List<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    public Forum Forum { get; set; } = new Forum();
    
    public Organization(string name, string email)
    {
        Name = name;
        Email = email;
    }
}