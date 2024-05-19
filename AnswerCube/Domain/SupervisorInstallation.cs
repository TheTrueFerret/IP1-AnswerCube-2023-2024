using Domain;

namespace AnswerCube.BL.Domain;

public class SupervisorInstallation
{
    public int Id { get; set; }
    public Installation? Installation { get; set; }
    public string Notes { get; set; }
}