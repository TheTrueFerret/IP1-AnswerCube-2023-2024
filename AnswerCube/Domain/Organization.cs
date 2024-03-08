namespace Domain;

public class Organization
{
    private string name { get; set; }
    private string email { get; set; }
    private List<Project> projects { get; set; }

    public Organization(string name, string email, List<Project> projects)
    {
        this.name = name;
        this.email = email;
        this.projects = projects;
    }
}