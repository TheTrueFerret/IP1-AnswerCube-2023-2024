namespace AnswerCube.UI.MVC.Models.Dto;

public class InstallationDto
{
    public int Id { get; set; }
    public string Location { get; set; }
    public bool Active { get; set; }
    public int CurrentSlideIndex { get; set; }
    public int MaxSlideIndex { get; set; }
}