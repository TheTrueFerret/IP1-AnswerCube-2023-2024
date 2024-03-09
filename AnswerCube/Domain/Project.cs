namespace Domain;

public class Project
{
    public String Name { get; set; }
    public String Description { get; set; }
    public Forum Forum { get; set; }
    public List<IFlow> Flows { get; set; }
    public Boolean IsActive { get; set; }

    public Project(String name, String description, Forum forum, List<IFlow> flows, Boolean isActive)
    {
        Name = name;
        Description = description;
        Forum = forum;
        Flows = flows;
        IsActive = isActive;
    }
}