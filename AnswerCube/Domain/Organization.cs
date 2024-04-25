namespace Domain;

public class Organization
{
    private string Name { get; set; }
    private string Email { get; set; }
    private ICollection<Project> Projects { get; set; }
    
    public Organization(string name, string email, List<Project> projects)
    {
        Name = name;
        Email = email;
        Projects = projects;
    }
}