using AnswerCube.UI.MVC.Models.Dto;
using Domain;

namespace AnswerCube.UI.MVC.Models;

public class InstallationModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Location { get; set; }
    public bool? Active { get; set; }
    public int? CurrentSlideIndex { get; set; }
    public int? MaxSlideIndex { get; set; }

    public InstallationModel(int id, string name, string? location, bool? active, int? currentSlideIndex, int? maxSlideIndex)
    {
        Id = id;
        Name = name;
        Location = location;
        Active = active;
        CurrentSlideIndex = currentSlideIndex;
        MaxSlideIndex = maxSlideIndex;
    }
}