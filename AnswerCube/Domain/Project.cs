namespace Domain;

public class Project
{
    private String name { get; set; }
    private String description { get; set; }
    private Forum _forum { get; set; }
    private List<Flow> flows { get; set; }
    private Boolean isActive { get; set; }
}