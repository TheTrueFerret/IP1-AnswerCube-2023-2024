namespace Domain;

public class Project
{
    private String name { get; set; }
    private String description { get; set; }
    private Forum _forum { get; set; }
    private List<IFlow> flows { get; set; }
    private Boolean isActive { get; set; }

    public Project(String name, String description, Forum forum, List<IFlow> flows, Boolean isActive)
    {
        this.name = name;
        this.description = description;
        this._forum = forum;
        this.flows = flows;
        this.isActive = isActive;
    }
}