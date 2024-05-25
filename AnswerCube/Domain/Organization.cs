using System.ComponentModel.DataAnnotations;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;

namespace Domain;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Theme Theme { get; set; }
    public string logoUrl { get; set; }
    public List<Project> Projects { get; set; } = new List<Project>();
    public List<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
    public List<Installation>? Installations { get; set; }
    public Forum Forum { get; set; } = new Forum();
    
    public Organization(string name, string email, string? logoUrl)
    {
        Name = name;
        Email = email;
        if(logoUrl == null)
        {
            this.logoUrl = "UI-MVC/wwwroot/Images/AnswerCubeLogo.png";
        }
    }
    
    public Organization(string name, string email, string? logoUrl, Theme theme)
    {
        Name = name;
        Email = email;
        Theme = theme;
        if(logoUrl == null)
        {
            this.logoUrl = "UI-MVC/wwwroot/Images/AnswerCubeLogo.png";
        }
    }
}