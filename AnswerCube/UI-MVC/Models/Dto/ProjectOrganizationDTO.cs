using AnswerCube.BL.Domain.Project;
using Domain;

namespace AnswerCube.UI.MVC.Models.Dto;

public class ProjectOrganizationDTO
{
    public Project Project { get; set; }
    public Organization Organization { get; set; }
}